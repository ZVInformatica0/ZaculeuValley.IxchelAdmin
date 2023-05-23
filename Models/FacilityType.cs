using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class FacilityType
{
    /// <summary>
    /// ID of facility type for Table FacilityType
    /// </summary>
    public int IdfacilityType { get; set; }

    /// <summary>
    /// Foreign key for Institution on FacilityType table
    /// </summary>
    public int Idinstitution { get; set; }

    /// <summary>
    /// Internal code for Facility Type (diff. each institution)
    /// </summary>
    /// 
    [Required(ErrorMessage = "Por favor ingrese el código para el tipo de instalación.")]
    public string? FacilityTypeCode { get; set; }

    /// <summary>
    /// Facility type name for table FacilityTypeName
    /// </summary>
    /// 
    [StringLength(50, ErrorMessage = "El nombre del tipo de instalación debe tener como máximo 50 caracteres.")]
    [Required(ErrorMessage = "Por favor ingrese el nombre del tipo de instalación.")]
    public string FacilityTypeName { get; set; } = null!;

    /// <summary>
    /// State of facility for FacilityType table
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// State of delete of a Facility for FacilityType Table
    /// </summary>
    public bool Deleted { get; set; }

    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();

    public virtual Institution IdinstitutionNavigation { get; set; } = null!;
}
