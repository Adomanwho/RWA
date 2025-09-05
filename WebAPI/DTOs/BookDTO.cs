using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class BookDTO
    {
        [Required(ErrorMessage = "Book name is required.")]
        [StringLength(200, ErrorMessage = "Book name cannot exceed 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author first name is required.")]
        [StringLength(100, ErrorMessage = "Author first name cannot exceed 100 characters.")]
        public string AuthorFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author last name is required.")]
        [StringLength(100, ErrorMessage = "Author last name cannot exceed 100 characters.")]
        public string AuthorLastName { get; set; } = string.Empty;

        [Range(1, 100000, ErrorMessage = "Number of pages must be greater than 0.")]
        public int NumberOfPages { get; set; }

        public int? GenreId { get; set; }

        [StringLength(200, ErrorMessage = "Genre name cannot exceed 200 characters.")]
        public string? GenreName { get; set; }
    }
}
