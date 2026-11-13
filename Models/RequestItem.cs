namespace RequestTS.Models
{
    public class RequestItem
    {
        public long Id { get; set; }

        public string RequestNo { get; set; } = "";

        public string Project { get; set; } = "";

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Priority { get; set; } = "Normal";

        public string Requester { get; set; } = "";

        public string? Assignee { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AssignedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateOnly? DueDate { get; set; }

        public List<RequestActivity> Activities { get; set; }
            = new List<RequestActivity>();
    }
}
