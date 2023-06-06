using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class User
{
    public int Iduser { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
