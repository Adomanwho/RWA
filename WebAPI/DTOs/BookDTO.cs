namespace WebAPI.DTOs
{
    public class BookDTO
    {
        public required string Name { get; set; }
        public required string AuthorFirstName { get; set; }
        public required string AuthorLastName { get; set; }
        public int NumberOfPages { get; set; }
        public int GenreId { get; set; }
    }
}
