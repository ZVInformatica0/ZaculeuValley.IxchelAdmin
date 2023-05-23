using System;
using System.Collections.Generic;

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
    public string? FacilityTypeCode { get; set; }

    /// <summary>
    /// Facility type name for table FacilityTypeName
    /// </summary>
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
