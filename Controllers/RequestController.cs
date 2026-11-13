using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestTS.Data;
using RequestTS.Models;
using RequestTS.ViewModels;

namespace RequestTS.Controllers;

public class RequestsController : Controller
{
    private readonly RequestTsDbContext _db;
    public RequestsController(RequestTsDbContext db) => _db = db;

    private async Task<List<RequestItem>> BuildItems()
    {
        var rows = await (from r in _db.Requests.AsNoTracking()
                          join p in _db.Projects.AsNoTracking() on r.ProjectId equals p.ProjectId
                          join u in _db.AppUsers.AsNoTracking() on r.RequestedBy equals u.UserId
                          select new { r, p, u }).ToListAsync();

        var assignments = await _db.RequestAssignments.AsNoTracking()
            .Where(a => a.EndedAt == null).ToListAsync();
        var users = await _db.AppUsers.AsNoTracking().ToDictionaryAsync(x => x.UserId, x => x.DisplayName);
        var comments = await _db.RequestComments.AsNoTracking().ToListAsync();

        return rows.Select(x =>
        {
            var active = assignments.Where(a => a.RequestId == x.r.RequestId)
                .OrderByDescending(a => a.AssignedAt).FirstOrDefault();
            return new RequestItem
            {
                Id = checked((int)x.r.RequestId),
                RequestNo = x.r.RequestNo,
                Project = x.p.ProjectName,
                Title = x.r.Title,
                Description = comments.Where(c => c.RequestId == x.r.RequestId)
                    .OrderBy(c => c.CreatedAt).Select(c => c.Body).FirstOrDefault() ?? "",
                Priority = x.r.PriorityCode,
                Requester = x.u.DisplayName,
                Assignee = active != null && users.TryGetValue(active.AssigneeUserId, out var name) ? name : null,
                Status = Enum.Parse<RequestStatus>(x.r.StatusCode),
                CreatedAt = x.r.SubmittedAt,
                AssignedAt = active?.AssignedAt,
                CompletedAt = x.r.CompletedAt,
                DueDate = x.r.DueDate,
                Activities = new List<RequestActivity>()
            };
        }).OrderByDescending(x => x.CreatedAt).ToList();
    }

    public async Task<IActionResult> Index(string? project, string? status, string? assignee)
    {
        var items = await BuildItems();
        var query = items.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(project)) query = query.Where(x => x.Project == project);
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RequestStatus>(status, out var parsed))
            query = query.Where(x => x.Status == parsed);
        if (!string.IsNullOrWhiteSpace(assignee))
            query = assignee == "Unassigned" ? query.Where(x => x.Assignee == null) : query.Where(x => x.Assignee == assignee);

        ViewBag.Projects = await _db.Projects.Where(x => x.IsActive).OrderBy(x => x.ProjectName)
            .Select(x => x.ProjectName).ToListAsync();
        ViewBag.Developers = await _db.AppUsers.Where(x => x.IsActive)
            .OrderBy(x => x.DisplayName).Select(x => x.DisplayName).ToListAsync();

        return View(new RequestFilterViewModel {
            Project = project, Status = status, Assignee = assignee, Requests = query.ToList()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCreateOptions();
        return View(new CreateRequestViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRequestViewModel model)
    {
        if (!ModelState.IsValid) { await LoadCreateOptions(); return View(model); }

        var project = await _db.Projects.FirstOrDefaultAsync(x => x.ProjectName == model.Project && x.IsActive);
        var requester = await _db.AppUsers.FirstOrDefaultAsync(x => x.IsActive);
        if (project == null || requester == null)
        {
            ModelState.AddModelError("", "Active Project နှင့် APP_USERS ထဲမှ Active User အနည်းဆုံးတစ်ဦး လိုအပ်ပါသည်။");
            await LoadCreateOptions(); return View(model);
        }

        var now = DateTime.Now;
        var request = new RequestEntity {
            RequestNo = $"REQ-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(100,999)}",
            ProjectId = project.ProjectId, RequestedBy = requester.UserId,
            Title = model.Title, PriorityCode = model.Priority,
            StatusCode = "Submitted", SubmittedAt = now, DueDate = model.DueDate
        };
        _db.Requests.Add(request);
        await _db.SaveChangesAsync();

        _db.RequestComments.Add(new RequestComment {
            RequestId = request.RequestId, AuthorUserId = requester.UserId,
            Body = model.Description, CreatedAt = now
        });
        _db.RequestStatusHistories.Add(new RequestStatusHistory {
            RequestId = request.RequestId, FromStatus = null, ToStatus = "Submitted",
            ChangedBy = requester.UserId, ChangedAt = now, Note = "Request created"
        });
        await _db.SaveChangesAsync();
        TempData["Success"] = "Request created and saved to RequestTsDB.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCreateOptions()
    {
        ViewBag.Projects = await _db.Projects.Where(x => x.IsActive)
            .OrderBy(x => x.ProjectName).Select(x => x.ProjectName).ToListAsync();
    }

    public async Task<IActionResult> Details(long id)
    {
        var item = (await BuildItems()).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();

        var req = await _db.Requests.FirstAsync(x => x.RequestId == id);
        var history = await _db.RequestStatusHistories.Where(x => x.RequestId == id)
            .OrderBy(x => x.ChangedAt).ToListAsync();
        var users = await _db.AppUsers.ToDictionaryAsync(x => x.UserId, x => x.DisplayName);
        item.Activities = history.Select(h => new RequestActivity {
            Actor = users.GetValueOrDefault(h.ChangedBy, "User"),
            Action = h.FromStatus == null ? "created this request" : $"changed status from {h.FromStatus} to {h.ToStatus}",
            Note = h.Note ?? "", CreatedAt = h.ChangedAt
        }).ToList();

        ViewBag.Developers = await (from m in _db.ProjectMembers
                                    join u in _db.AppUsers on m.UserId equals u.UserId
                                    where m.ProjectId == req.ProjectId && m.IsActive && u.IsActive
                                    orderby u.DisplayName
                                    select u.DisplayName).ToArrayAsync();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(long id, string assignee, string? note)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(x => x.RequestId == id);
        if (request == null) return NotFound();

        var member = await (from m in _db.ProjectMembers
                            join u in _db.AppUsers on m.UserId equals u.UserId
                            where m.ProjectId == request.ProjectId && m.IsActive && u.IsActive && u.DisplayName == assignee
                            select new { m.UserId }).FirstOrDefaultAsync();
        if (member == null) { TempData["Error"] = "ရွေးချယ်ထားသော user သည် ဤ project ၏ active member မဟုတ်ပါ။"; return RedirectToAction(nameof(Details), new { id }); }

        var actor = await _db.AppUsers.Where(x => x.IsActive).Select(x => x.UserId).FirstAsync();
        var now = DateTime.Now;
        var previous = await _db.RequestAssignments.Where(x => x.RequestId == id && x.EndedAt == null)
            .OrderByDescending(x => x.AssignedAt).FirstOrDefaultAsync();
        if (previous != null) { previous.EndedAt = now; previous.EndedBy = actor; }

        var assignment = new RequestAssignment {
            RequestId = id, ProjectId = request.ProjectId, AssigneeUserId = member.UserId,
            AssignedBy = actor, AssignedAt = now, AssignmentNote = note
        };
        _db.RequestAssignments.Add(assignment);
        var old = request.StatusCode;
        if (request.StatusCode == "Submitted") request.StatusCode = "Assigned";
        await _db.SaveChangesAsync();
        _db.RequestStatusHistories.Add(new RequestStatusHistory {
            RequestId = id, AssignmentId = assignment.AssignmentId, FromStatus = old,
            ToStatus = request.StatusCode, ChangedBy = actor, ChangedAt = now, Note = note ?? "Assigned"
        });
        await _db.SaveChangesAsync();
        TempData["Success"] = "Assignment saved.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(long id, RequestStatus status, string? note)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(x => x.RequestId == id);
        if (request == null) return NotFound();
        var next = status.ToString();
        if (!new[] { "Submitted", "Assigned", "InProgress", "OnHold", "Completed", "Cancelled" }.Contains(next))
            return BadRequest();

        var activeAssignment = await _db.RequestAssignments.AnyAsync(x => x.RequestId == id && x.EndedAt == null);
        if (next == "Completed" && !activeAssignment) {
            TempData["Error"] = "Complete မလုပ်ခင် developer တစ်ဦး assign လုပ်ပါ။";
            return RedirectToAction(nameof(Details), new { id });
        }

        var actor = await _db.AppUsers.Where(x => x.IsActive).Select(x => x.UserId).FirstAsync();
        var old = request.StatusCode;
        request.StatusCode = next;
        if (next == "InProgress" && request.FirstStartedAt == null) request.FirstStartedAt = DateTime.Now;
        request.CompletedAt = next == "Completed" ? DateTime.Now : null;
        _db.RequestStatusHistories.Add(new RequestStatusHistory {
            RequestId = id, FromStatus = old, ToStatus = next,
            ChangedBy = actor, ChangedAt = DateTime.Now, Note = note
        });
        await _db.SaveChangesAsync();
        TempData["Success"] = "Status updated.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
