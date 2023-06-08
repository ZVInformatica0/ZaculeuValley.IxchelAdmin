using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Rol
{
    public int Idrol { get; set; }

    [Required(ErrorMessage ="El nombre debe de ser obligatorio")]
    public string? RolName { get; set; }

    [Required(ErrorMessage = "La descripcion debe de ser obligatoria")]

    public string? RolDescription { get; set; }

    public virtual ICollection<PermissionRol> PermissionRols { get; set; } = new List<PermissionRol>();

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
