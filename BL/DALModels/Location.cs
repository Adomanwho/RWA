using System;
using System.Collections.Generic;

namespace BL.DALModels;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<LocationBook> LocationBooks { get; set; } = new List<LocationBook>();
}
