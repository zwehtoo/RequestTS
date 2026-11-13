using Microsoft.AspNetCore.Mvc;

namespace RequestTS.Models
{
    public class RequestActivity
    {
        public string Actor { get; set; } = "";

        public string Action { get; set; } = "";

        public string Note { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
