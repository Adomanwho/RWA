using BL.DALModels;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Password should be at least 5 characters long")]
        public string Password { get; set; } = null!;
    }
}
