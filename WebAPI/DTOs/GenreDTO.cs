using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class GenreDTO
    {
        [StringLength(200, ErrorMessage = "Genre name cannot exceed 200 characters.")]
        public string Name { get; set; }
    }
}
