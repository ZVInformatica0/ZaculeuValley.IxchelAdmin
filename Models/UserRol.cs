using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class UserRol
{
    public int IduserRol { get; set; }

    public int? Iduser { get; set; }

    public string? UserRolName { get; set; }

    public string? UserRolDescription { get; set; }

    public virtual User? IduserNavigation { get; set; }
}
