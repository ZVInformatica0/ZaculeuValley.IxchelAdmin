using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Permission
{
    public int Idpermission { get; set; }

    [Required(ErrorMessage ="El nombre debe ser obligatorio")]
    public string? PermissionName { get; set; }

    [Required(ErrorMessage = "La descripcion debe ser obligatoria")]
    public string? PermissionDescription { get; set; }

    public virtual ICollection<PermissionRol> PermissionRols { get; set; } = new List<PermissionRol>();
}
