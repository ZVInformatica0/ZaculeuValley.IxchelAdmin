using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Rol
{
    public int Idrol { get; set; }

    public string? RolName { get; set; }

    public string? RolDescription { get; set; }

    public virtual ICollection<PermissionRol> PermissionRols { get; set; } = new List<PermissionRol>();

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
