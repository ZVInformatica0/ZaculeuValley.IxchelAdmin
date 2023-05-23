using System;
using System.Collections.Generic;

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

    public int? CountryCode { get; set; }

    /// <summary>
    /// Name for Table Institution Country
    /// </summary>
    public string CountryName { get; set; } = null!;

    /// <summary>
    /// Phone code for Table Institution Country
    /// </summary>
    public string? CountryPhoneCode { get; set; }

    /// <summary>
    /// Domain ectention for the table Institution Country
    /// </summary>
    public string? CountryDomainName { get; set; }

    public bool Enabled { get; set; }

    public bool Deleted { get; set; }

    public virtual Institution? IdinstitutionNavigation { get; set; }

    public virtual ICollection<InstitutionArea> InstitutionAreas { get; set; } = new List<InstitutionArea>();
}
