using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class InstitutionArea
{
    /// <summary>
    /// Id for table Institution Area
    /// </summary>
    public int IdinstitutionArea { get; set; }

    /// <summary>
    /// Id Foreign for Institution for Area Table
    /// </summary>
    public int? Idinstitution { get; set; }

    /// <summary>
    /// Id for table Institution Country (Foreign Key)
    /// </summary>
    public int IdinstitutionCountry { get; set; }

    /// <summary>
    /// Internal code for Table Institution Area
    /// </summary>
    public string AreaCode { get; set; } = null!;

    /// <summary>
    /// Name of the area for table Institution Area
    /// </summary>
    public string AreaName { get; set; } = null!;

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual InstitutionCountry IdinstitutionCountryNavigation { get; set; } = null!;

    public virtual Institution? IdinstitutionNavigation { get; set; }

    public virtual ICollection<InstitutionDistrict> InstitutionDistricts { get; set; } = new List<InstitutionDistrict>();
}
