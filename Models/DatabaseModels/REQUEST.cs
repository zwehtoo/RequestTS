using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class REQUEST
{
    public long request_id { get; set; }

    public string request_no { get; set; } = null!;

    public long project_id { get; set; }

    public long requested_by { get; set; }

    public string title { get; set; } = null!;

    public string priority_code { get; set; } = null!;

    public string status_code { get; set; } = null!;

    public DateTime submitted_at { get; set; }

    public DateTime? first_started_at { get; set; }

    public DateTime? completed_at { get; set; }

    public DateOnly? due_date { get; set; }

    public virtual ICollection<REQUEST_ASSIGNMENT> REQUEST_ASSIGNMENTs { get; set; } = new List<REQUEST_ASSIGNMENT>();

    public virtual ICollection<REQUEST_ATTACHMENT> REQUEST_ATTACHMENTs { get; set; } = new List<REQUEST_ATTACHMENT>();

    public virtual ICollection<REQUEST_COMMENT> REQUEST_COMMENTs { get; set; } = new List<REQUEST_COMMENT>();

    public virtual ICollection<REQUEST_STATUS_HISTORY> REQUEST_STATUS_HISTORies { get; set; } = new List<REQUEST_STATUS_HISTORY>();

    public virtual PROJECT project { get; set; } = null!;

    public virtual APP_USER requested_byNavigation { get; set; } = null!;
}
