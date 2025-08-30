using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLBook
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string AuthorFirstName { get; set; } = null!;

        public string AuthorLastName { get; set; } = null!;

        public int NumberOfPages { get; set; }

        public int GenreId { get; set; }
    }
}
