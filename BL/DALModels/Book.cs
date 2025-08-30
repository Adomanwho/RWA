using System;
using System.Collections.Generic;

namespace BL.DALModels;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string AuthorFirstName { get; set; } = null!;

    public string AuthorLastName { get; set; } = null!;

    public int NumberOfPages { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<LocationBook> LocationBooks { get; set; } = new List<LocationBook>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
