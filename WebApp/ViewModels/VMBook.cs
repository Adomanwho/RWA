using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMBook
    {
        [Required]
        [Display(Name = "Book Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Authors First Name")]
        public string AuthorFirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Authors Last Name")]
        public string AuthorLastName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be greater than 0")]
        [Display(Name = "Number Of Pages")]
        public int NumberOfPages { get; set; }

        [Display(Name = "Genre")]
        public string? GenreName { get; set; }

        [Required(ErrorMessage = "You must select a genre")]
        public int GenreId { get; set; }
    }
}
