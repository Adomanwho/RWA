using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLLocation
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<LocationBook> LocationBooks { get; set; } = new List<LocationBook>();
    }
}
