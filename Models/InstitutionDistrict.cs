using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class InstitutionDistrict
{
    /// <summary>
    /// Id for table Institution District
    /// </summary>
    public int IdinstitutionDistrict { get; set; }

    /// <summary>
    /// Id for table Institution District (Foreign Key)
    /// </summary>
    public int IdinstitutionArea { get; set; }

    /// <summary>
    /// Id Foreign for Institution for DistrictTable
    /// </summary>
    public int? Idinstitution { get; set; }

    public string? AreaCode { get; set; }

    /// <summary>
    /// Internal code for table Institution District
    /// </summary>
    /// 
    [Required(ErrorMessage = "Por favor ingrese el código del Distrito.")]
    public int? DistrictCode { get; set; }

    /// <summary>
    /// Name for table Institution District
    /// </summary>
    /// 
    [StringLength(50, ErrorMessage = "El nombre del Distrito debe tener como máximo 50 caracteres.")]
    [Required(ErrorMessage = "Por favor ingrese el nombre del Distrito.")]
    public string DistrictName { get; set; } = null!;

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();

    public virtual InstitutionArea IdinstitutionAreaNavigation { get; set; } = null!;

    public virtual Institution? IdinstitutionNavigation { get; set; }


}
