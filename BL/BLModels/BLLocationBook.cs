using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLLocationBook
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int LocationId { get; set; }

        public string Name { get; set; } = null!;

        public virtual Book Book { get; set; } = null!;

        public virtual Location Location { get; set; } = null!;
    }
}
