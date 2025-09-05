using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class VMBookList
    {
        public IEnumerable<VMBook> Books { get; set; } = new List<VMBook>();

        public string? Search { get; set; }
        public int? SelectedGenreId { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => (Page * PageSize) < TotalCount;
    }
}
