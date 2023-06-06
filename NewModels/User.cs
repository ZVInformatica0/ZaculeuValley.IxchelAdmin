using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.NewModels;

public partial class User
{
    public int Iduser { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int? UserState { get; set; }

    public bool UserStateSession { get; set; }

    public bool UserEnable { get; set; }

    public bool? UserDeleted { get; set; }

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
