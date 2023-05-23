using System;
using System.Collections.Generic;

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
    public int? DistrictCode { get; set; }

    /// <summary>
    /// Name for table Institution District
    /// </summary>
    public string DistrictName { get; set; } = null!;

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();

    public virtual InstitutionArea IdinstitutionAreaNavigation { get; set; } = null!;

    public virtual Institution? IdinstitutionNavigation { get; set; }


}
