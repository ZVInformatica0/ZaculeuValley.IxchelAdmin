using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Facility
{
    /// <summary>
    /// Id for table Facility
    /// </summary>
    public int Idfacility { get; set; }

    /// <summary>
    /// Id Foreign for Institution for Facility Table
    /// </summary>
    public int? Idinstitution { get; set; }

    /// <summary>
    /// Id for District (Foreign key) for table Facility
    /// </summary>
    public int Iddistrict { get; set; }

    /// <summary>
    /// Internal Code for table Facility
    /// </summary>
    public string? FacilityCode { get; set; }

    /// <summary>
    /// Id for type of facility for Facility table
    /// </summary>
    public int? IdfacilityType { get; set; }

    /// <summary>
    /// Name for Facility table
    /// </summary>
    public string FacilityName { get; set; } = null!;

    /// <summary>
    /// State of facility for Facility table
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// State of delete of a Facility for Facility Table
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Internal code id for area
    /// </summary>
    public string? AreaCode { get; set; }

    /// <summary>
    /// Internal code id for district
    /// </summary>
    public string? DistrictCode { get; set; }

    public virtual InstitutionDistrict IddistrictNavigation { get; set; } = null!;

    public virtual FacilityType? IdfacilityTypeNavigation { get; set; }

    public virtual Institution? IdinstitutionNavigation { get; set; }
}
