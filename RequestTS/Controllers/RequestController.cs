

using Microsoft.AspNetCore.Mvc;
using RequestTS.Models;
using RequestTS.ViewModels;

namespace WidgetMVC.Controllers
{
    public class RequestsController : Controller
    {
        private readonly List<RequestItem> _requests =
            SampleData.Requests;

        // 1. REQUEST LIST
        public IActionResult Index(
            string? project,
            string? status,
            string? assignee)
        {
            var query = _requests.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(project))
            {
                query = query.Where(x => x.Project == project);
            }

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<RequestStatus>(status, out var parsedStatus))
            {
                query = query.Where(x => x.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(assignee))
            {
                if (assignee == "Unassigned")
                {
                    query = query.Where(x => x.Assignee == null);
                }
                else
                {
                    query = query.Where(x => x.Assignee == assignee);
                }
            }

            var model = new RequestFilterViewModel
            {
                Project = project,
                Status = status,
                Assignee = assignee,
                Requests = query.OrderBy(x => x.Id).ToList()
            };

            return View(model);
        }

        // 2. CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRequestViewModel());
        }

        // 3. CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int newId = _requests.Count == 0
                ? 1
                : _requests.Max(x => x.Id) + 1;

            var request = new RequestItem
            {
                Id = newId,
                RequestNo = $"REQ-{1000 + newId}",
                Project = model.Project,
                Title = model.Title,
                Description = model.Description,
                Priority = model.Priority,
                Requester = "May Thu",
                Status = RequestStatus.Submitted,
                CreatedAt = DateTime.Now,
                DueDate = model.DueDate
            };

            request.Activities.Add(new RequestActivity
            {
                Actor = "May Thu",
                Action = "created this request",
                CreatedAt = DateTime.Now
            });

            _requests.Add(request);

            TempData["Success"] = "Request created successfully!";

            return RedirectToAction(nameof(Index));
        }

        // 4. DETAILS
        public IActionResult Details(int id)
        {
            var request = _requests.FirstOrDefault(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            ViewBag.Developers = SampleData.Developers;

            return View(request);
        }

        // 5. ASSIGN / REASSIGN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(
            int id,
            string assignee,
            string? note)
        {
            var request = _requests.FirstOrDefault(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (!SampleData.Developers.Contains(assignee))
            {
                return BadRequest();
            }

            request.Assignee = assignee;
            request.AssignedAt = DateTime.Now;

            if (request.Status == RequestStatus.Submitted)
            {
                request.Status = RequestStatus.Assigned;
            }

            request.Activities.Add(new RequestActivity
            {
                Actor = "May Thu",
                Action = $"assigned to {assignee}",
                Note = note ?? "",
                CreatedAt = DateTime.Now
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        // 6. CHANGE STATUS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(
            int id,
            RequestStatus status,
            string? note)
        {
            var request = _requests.FirstOrDefault(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (!Enum.IsDefined(status))
            {
                return BadRequest();
            }

            // Demo workflow restrictions
            if (request.Status == RequestStatus.Cancelled &&
                status != RequestStatus.Submitted)
            {
                return BadRequest();
            }

            if (request.Status == RequestStatus.Completed &&
                status != RequestStatus.Submitted)
            {
                return BadRequest();
            }

            if (status == RequestStatus.Completed &&
                request.Assignee == null)
            {
                TempData["Error"] =
                    "Please assign a developer first.";

                return RedirectToAction(nameof(Details), new { id });
            }

            request.Status = status;

            if (status == RequestStatus.Completed)
            {
                request.CompletedAt = DateTime.Now;
            }
            else
            {
                request.CompletedAt = null;
            }

            request.Activities.Add(new RequestActivity
            {
                Actor = "May Thu",
                Action = $"changed status to {status}",
                Note = note ?? "",
                CreatedAt = DateTime.Now
            });

            TempData["Success"] = "Status updated successfully!";

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
