using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMLocationBook
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book name is required.")]
        [StringLength(200, ErrorMessage = "Book name cannot exceed 200 characters.")]
        [Display(Name = "Book Name")]
        public string BookName { get; set; } = null!;

        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(200, ErrorMessage = "Location name cannot exceed 200 characters.")]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; } = null!;

        public string? Message { get; set; }
    }
}
