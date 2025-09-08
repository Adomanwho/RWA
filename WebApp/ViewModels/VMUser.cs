using BL.DALModels;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMUser
    {
        public int Id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(150, ErrorMessage = "Username cannot be longer than 200 characters.")]
        [MinLength(1, ErrorMessage = "Username cannot be empty.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(200, ErrorMessage = "Password cannot be longer than 250 characters.")]
        [MinLength(1, ErrorMessage = "Password cannot be empty.")]
        public string Password { get; set; } = null!;
        public int? RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual Role Role { get; set; } = null!;
    }
}
