using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class Log
{
    public long Idlog { get; set; }

    public string AlteredTable { get; set; } = null!;

    public string Action { get; set; } = null!;

    public int AlteredBy { get; set; }

    public DateTime AlterationDate { get; set; }

    public string? Message { get; set; }
}
