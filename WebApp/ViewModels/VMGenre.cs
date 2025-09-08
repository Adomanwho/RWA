using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMGenre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Genre name is required.")]
        [MaxLength(100, ErrorMessage = "Genre name cannot be longer than 100 characters.")]
        [Display(Name = "Genre Name")]
        public string Name { get; set; } = null!;
    }
}
