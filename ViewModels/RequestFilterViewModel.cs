using RequestTS.Models;

namespace RequestTS.ViewModels
{
    public class RequestFilterViewModel
    {
        public string? Project { get; set; }

        public string? Status { get; set; }

        public string? Assignee { get; set; }

        public List<RequestItem> Requests { get; set; }
            = new List<RequestItem>();
    }
}
