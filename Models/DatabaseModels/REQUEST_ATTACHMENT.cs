using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class REQUEST_ATTACHMENT
{
    public long attachment_id { get; set; }

    public long request_id { get; set; }

    public string original_file_name { get; set; } = null!;

    public string storage_key { get; set; } = null!;

    public long uploaded_by { get; set; }

    public DateTime uploaded_at { get; set; }

    public virtual REQUEST request { get; set; } = null!;

    public virtual APP_USER uploaded_byNavigation { get; set; } = null!;
}
