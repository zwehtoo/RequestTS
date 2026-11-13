using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class REQUEST_COMMENT
{
    public long comment_id { get; set; }

    public long request_id { get; set; }

    public long author_user_id { get; set; }

    public string body { get; set; } = null!;

    public DateTime created_at { get; set; }

    public virtual APP_USER author_user { get; set; } = null!;

    public virtual REQUEST request { get; set; } = null!;
}
