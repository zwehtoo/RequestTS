using Microsoft.AspNetCore.Mvc;
using RequestTS.Models;

namespace RequestTS.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            var requests = SampleData.Requests;

            ViewBag.Total = requests.Count;

            ViewBag.Submitted = requests.Count(
                x => x.Status == RequestStatus.Submitted);

            ViewBag.Assigned = requests.Count(
                x => x.Status == RequestStatus.Assigned);

            ViewBag.InProgress = requests.Count(
                x => x.Status == RequestStatus.InProgress);

            ViewBag.Completed = requests.Count(
                x => x.Status == RequestStatus.Completed);

            ViewBag.OnHold = requests.Count(
                x => x.Status == RequestStatus.OnHold);

            return View(requests);
        }
    }
}
