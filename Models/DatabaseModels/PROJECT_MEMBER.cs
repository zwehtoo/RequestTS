using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class PROJECT_MEMBER
{
    public long project_id { get; set; }

    public long user_id { get; set; }

    public string project_role { get; set; } = null!;

    public bool is_active { get; set; }

    public virtual ICollection<REQUEST_ASSIGNMENT> REQUEST_ASSIGNMENTs { get; set; } = new List<REQUEST_ASSIGNMENT>();

    public virtual PROJECT project { get; set; } = null!;

    public virtual APP_USER user { get; set; } = null!;
}
