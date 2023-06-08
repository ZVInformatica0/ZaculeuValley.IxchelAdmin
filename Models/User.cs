using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class User
{
    public int Iduser { get; set; }
    [Required(ErrorMessage ="El nombre de usuario es obligatorio")]
    public string UserName { get; set; } = null!;

    [StringLength(8, ErrorMessage ="Su contrasena debe ser de 8 caracteres")]
    public string UserPassword { get; set; } = null!;

    public int? UserState { get; set; }

    public bool UserStateSession { get; set; }

    public bool UserEnable { get; set; }

    public bool? UserDeleted { get; set; }

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
