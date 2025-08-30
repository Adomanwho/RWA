using System;
using System.Collections.Generic;

namespace BL.DALModels;

public partial class LocationBook
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
