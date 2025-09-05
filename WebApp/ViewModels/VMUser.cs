using BL.DALModels;

namespace WebApp.ViewModels
{
    public class VMUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual Role Role { get; set; } = null!;
    }
}
