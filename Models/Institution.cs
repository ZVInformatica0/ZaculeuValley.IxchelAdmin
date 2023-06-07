using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Institution
{
    /// <summary>
    /// Id fot table Institution
    /// </summary>
    /// 

    public int Idinstitution { get; set; }

    /// <summary>
    /// Name for table Institution
    /// </summary>
    /// 
    [StringLength(50, ErrorMessage = "El nombre de la institución debe tener como máximo 50 caracteres.")]
    [Required(ErrorMessage = "Por favor ingrese el nombre de la institución.")]
    public string InstitutionName { get; set; } = null!;

    /// <summary>
    /// Internal code for table Institution
    /// </summary>
    /// 

    [Required(ErrorMessage = "Por favor ingrese el código de la institución.")]
    public string InstitutionCode { get; set; } = null!;

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }



    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();

    public virtual ICollection<FacilityType> FacilityTypes { get; set; } = new List<FacilityType>();

    public virtual ICollection<InstitutionArea> InstitutionAreas { get; set; } = new List<InstitutionArea>();

    public virtual ICollection<InstitutionCountry> InstitutionCountries { get; set; } = new List<InstitutionCountry>();

    public virtual ICollection<InstitutionDistrict> InstitutionDistricts { get; set; } = new List<InstitutionDistrict>();
}
