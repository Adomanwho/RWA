using System;
using System.Collections.Generic;

namespace BL.DALModels;

public partial class Log
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public int LogLevel { get; set; }

    public string LogMessage { get; set; } = null!;

    public string? ErrorMessage { get; set; }
}
