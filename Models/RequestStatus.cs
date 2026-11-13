using Microsoft.AspNetCore.Mvc;

namespace RequestTS.Models
{
    public enum RequestStatus
    {
        Submitted,
        Assigned,
        InProgress,
        OnHold,
        Completed,
        Cancelled
    }
}
