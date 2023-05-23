using System;
using System.Collections.Generic;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class LogError
{
    public long Idlogerror { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Class { get; set; }

    public string? Function { get; set; }

    public string? ExMessage { get; set; }

    public string? ExStacktrace { get; set; }

    public string? Message { get; set; }
}
