using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Permission
{
    public int Idpermission { get; set; }

    public string? PermissionName { get; set; }

    public string? PermissionDescription { get; set; }

    public virtual ICollection<PermissionRol> PermissionRols { get; set; } = new List<PermissionRol>();
}
