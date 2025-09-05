using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMBook
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string AuthorFirstName { get; set; } = null!;

        [Required]
        public string AuthorLastName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be greater than 0")]
        public int NumberOfPages { get; set; }

        public string? GenreName { get; set; }

        [Required(ErrorMessage = "You must select a genre")]
        public int GenreId { get; set; }
    }
}
