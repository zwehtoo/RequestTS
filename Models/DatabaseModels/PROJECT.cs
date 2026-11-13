using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class PROJECT
{
    public long project_id { get; set; }

    public string project_code { get; set; } = null!;

    public string project_name { get; set; } = null!;

    public long created_by { get; set; }

    public bool is_active { get; set; }

    public virtual ICollection<PROJECT_MEMBER> PROJECT_MEMBERs { get; set; } = new List<PROJECT_MEMBER>();

    public virtual ICollection<REQUEST> REQUESTs { get; set; } = new List<REQUEST>();

    public virtual APP_USER created_byNavigation { get; set; } = null!;
}
