using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class REQUEST_STATUS_HISTORY
{
    public long status_event_id { get; set; }

    public long request_id { get; set; }

    public long? assignment_id { get; set; }

    public string? from_status { get; set; }

    public string to_status { get; set; } = null!;

    public long changed_by { get; set; }

    public DateTime changed_at { get; set; }

    public string? note { get; set; }

    public virtual REQUEST_ASSIGNMENT? REQUEST_ASSIGNMENT { get; set; }

    public virtual APP_USER changed_byNavigation { get; set; } = null!;

    public virtual REQUEST request { get; set; } = null!;
}
