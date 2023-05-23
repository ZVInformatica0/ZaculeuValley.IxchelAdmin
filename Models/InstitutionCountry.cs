using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class InstitutionCountry
{
    /// <summary>
    /// Id for the table Institution Country
    /// </summary>
    public int IdinstitutionCountry { get; set; }

    /// <summary>
    /// Id for table Institution Country (Foreign Key)
    /// </summary>
    public int? Idinstitution { get; set; }

    [Required(ErrorMessage = "Por favor ingrese el código del país.")]
    public int? CountryCode { get; set; }

    /// <summary>
    /// Name for Table Institution Country
    /// </summary>
    /// 
    [StringLength(50, ErrorMessage = "El nombre de la institución debe tener como máximo 50 caracteres.")]
    [Required(ErrorMessage = "Por favor ingrese el nombre de la institución.")]
    public string CountryName { get; set; } = null!;

    /// <summary>
    /// Phone code for Table Institution Country
    /// </summary>
    /// 
    [StringLength(4, ErrorMessage = "El prefijo teléfonico debe tener como máximo 4 caracteres con el simbolo +.")]
    public string? CountryPhoneCode { get; set; }

    /// <summary>
    /// Domain ectention for the table Institution Country
    /// </summary>
    /// 
    [StringLength(4, ErrorMessage = "El dominio debe tener como máximo 3 caracteres con el simbolo (Ej.: .gt)")]
    public string? CountryDomainName { get; set; }

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual Institution? IdinstitutionNavigation { get; set; }

    public virtual ICollection<InstitutionArea> InstitutionAreas { get; set; } = new List<InstitutionArea>();
}
