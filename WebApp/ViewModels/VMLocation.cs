using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMLocation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location name is required.")]
        [MaxLength(200, ErrorMessage = "Location name cannot be longer than 200 characters.")]
        [Display(Name = "Location Name")]
        public string Name { get; set; } = null!;
    }
}
