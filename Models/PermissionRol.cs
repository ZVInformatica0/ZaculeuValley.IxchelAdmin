using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class PermissionRol
{
    public int? Idrol { get; set; }

    public int? Idpermission { get; set; }

    public bool? Read { get; set; }

    public bool? Add { get; set; }

    public bool? Write { get; set; }

    public bool? Delete { get; set; }

    public bool? Print { get; set; }

    public virtual Permission? IdpermissionNavigation { get; set; }

    public virtual Rol? IdrolNavigation { get; set; }
}
