
using System.ComponentModel.DataAnnotations;

namespace RequestTS.ViewModels
{
    public class RequestEditViewModel
    {
        public long request_id { get; set; }

        [Required(ErrorMessage = "Please select a project.")]
        public long project_id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string title { get; set; } = "";

        [Required(ErrorMessage = "Priority is required.")]
        [StringLength(20)]
        public string priority_code { get; set; } = "Normal";

        public DateOnly? due_date { get; set; }
    }
}
