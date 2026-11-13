using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class REQUEST_ASSIGNMENT
{
    public long assignment_id { get; set; }

    public long request_id { get; set; }

    public long project_id { get; set; }

    public long assignee_user_id { get; set; }

    public long assigned_by { get; set; }

    public DateTime assigned_at { get; set; }

    public DateTime? ended_at { get; set; }

    public long? ended_by { get; set; }

    public string? assignment_note { get; set; }

    public virtual PROJECT_MEMBER PROJECT_MEMBER { get; set; } = null!;

    public virtual REQUEST REQUEST { get; set; } = null!;

    public virtual ICollection<REQUEST_STATUS_HISTORY> REQUEST_STATUS_HISTORies { get; set; } = new List<REQUEST_STATUS_HISTORY>();

    public virtual APP_USER assigned_byNavigation { get; set; } = null!;

    public virtual APP_USER? ended_byNavigation { get; set; }
}
