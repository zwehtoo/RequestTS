using System;
using System.Collections.Generic;

namespace RequestTS.Models.DatabaseModels;

public partial class APP_USER
{
    public long user_id { get; set; }

    public string display_name { get; set; } = null!;

    public string email { get; set; } = null!;

    public string global_role { get; set; } = null!;

    public bool is_active { get; set; }

    public virtual ICollection<PROJECT_MEMBER> PROJECT_MEMBERs { get; set; } = new List<PROJECT_MEMBER>();

    public virtual ICollection<PROJECT> PROJECTs { get; set; } = new List<PROJECT>();

    public virtual ICollection<REQUEST_ASSIGNMENT> REQUEST_ASSIGNMENTassigned_byNavigations { get; set; } = new List<REQUEST_ASSIGNMENT>();

    public virtual ICollection<REQUEST_ASSIGNMENT> REQUEST_ASSIGNMENTended_byNavigations { get; set; } = new List<REQUEST_ASSIGNMENT>();

    public virtual ICollection<REQUEST_ATTACHMENT> REQUEST_ATTACHMENTs { get; set; } = new List<REQUEST_ATTACHMENT>();

    public virtual ICollection<REQUEST_COMMENT> REQUEST_COMMENTs { get; set; } = new List<REQUEST_COMMENT>();

    public virtual ICollection<REQUEST_STATUS_HISTORY> REQUEST_STATUS_HISTORies { get; set; } = new List<REQUEST_STATUS_HISTORY>();

    public virtual ICollection<REQUEST> REQUESTs { get; set; } = new List<REQUEST>();
}
