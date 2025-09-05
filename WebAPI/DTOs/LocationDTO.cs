using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class LocationDTO
    {
        [StringLength(200, ErrorMessage = "Location name cannot exceed 200 characters.")]
        public string Name { get; set; }
    }
}
