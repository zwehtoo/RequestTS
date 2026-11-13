using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestTS.Data;
using RequestTS.Models;

namespace RequestTS.Controllers;

public class ReportsController : Controller
{
    private readonly RequestTsDbContext _db;
    public ReportsController(RequestTsDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var rows = await (from r in _db.Requests.AsNoTracking()
                          join p in _db.Projects.AsNoTracking() on r.ProjectId equals p.ProjectId
                          select new RequestItem {
                              Id = r.RequestId, Project = p.ProjectName,
                              Status = Enum.Parse<RequestStatus>(r.StatusCode)
                          }).ToListAsync();
        ViewBag.Total = rows.Count;
        ViewBag.Submitted = rows.Count(x => x.Status == RequestStatus.Submitted);
        ViewBag.Assigned = rows.Count(x => x.Status == RequestStatus.Assigned);
        ViewBag.InProgress = rows.Count(x => x.Status == RequestStatus.InProgress);
        ViewBag.Completed = rows.Count(x => x.Status == RequestStatus.Completed);
        ViewBag.OnHold = rows.Count(x => x.Status == RequestStatus.OnHold);
        return View(rows);
    }
}
