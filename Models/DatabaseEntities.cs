namespace RequestTS.Models;

public class AppUser
{
    public long UserId { get; set; }
    public string DisplayName { get; set; } = "";
    public string Email { get; set; } = "";
    public string GlobalRole { get; set; } = "";
    public bool IsActive { get; set; }
}
public class ProjectEntity
{
    public long ProjectId { get; set; }
    public string ProjectCode { get; set; } = "";
    public string ProjectName { get; set; } = "";
    public long CreatedBy { get; set; }
    public bool IsActive { get; set; }
}
public class ProjectMember
{
    public long ProjectId { get; set; }
    public long UserId { get; set; }
    public string ProjectRole { get; set; } = "";
    public bool IsActive { get; set; }
}
public class RequestEntity
{
    public long RequestId { get; set; }
    public string RequestNo { get; set; } = "";
    public long ProjectId { get; set; }
    public long RequestedBy { get; set; }
    public string Title { get; set; } = "";
    public string PriorityCode { get; set; } = "Normal";
    public string StatusCode { get; set; } = "Submitted";
    public DateTime SubmittedAt { get; set; }
    public DateTime? FirstStartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateOnly? DueDate { get; set; }
}
public class RequestAssignment
{
    public long AssignmentId { get; set; }
    public long RequestId { get; set; }
    public long ProjectId { get; set; }
    public long AssigneeUserId { get; set; }
    public long AssignedBy { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public long? EndedBy { get; set; }
    public string? AssignmentNote { get; set; }
}
public class RequestAttachment
{
    public long AttachmentId { get; set; }
    public long RequestId { get; set; }
    public string OriginalFileName { get; set; } = "";
    public string StorageKey { get; set; } = "";
    public long UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
}
public class RequestComment
{
    public long CommentId { get; set; }
    public long RequestId { get; set; }
    public long AuthorUserId { get; set; }
    public string Body { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
public class RequestStatusHistory
{
    public long StatusEventId { get; set; }
    public long RequestId { get; set; }
    public long? AssignmentId { get; set; }
    public string? FromStatus { get; set; }
    public string ToStatus { get; set; } = "";
    public long ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}
