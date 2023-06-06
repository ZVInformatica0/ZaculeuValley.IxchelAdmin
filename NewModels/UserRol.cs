using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.NewModels;

public partial class UserRol
{
    public int IduserRol { get; set; }

    public int? Iduser { get; set; }

    public int? Idrol { get; set; }

    public virtual Rol? IdrolNavigation { get; set; }

    public virtual User? IduserNavigation { get; set; }
}
