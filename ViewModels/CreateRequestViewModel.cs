using
    System.ComponentModel.DataAnnotations;
namespace RequestTS.ViewModels
{
    public class CreateRequestViewModel
    {
        [Required(ErrorMessage = "Project is required")]
        public string Project { get; set; } = "";

        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; } = "Normal";

        [Required(ErrorMessage = "Title is required")]
        [StringLength(150)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000)]
        public string Description { get; set; } = "";

        public DateOnly? DueDate { get; set; }
    }
}
