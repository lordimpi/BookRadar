using BookRadar.Common.DTOs;

namespace BookRadar.WebAPP.Models
{
    public class SearchViewModel
    {
        public string Author { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<LibroDTO> Items { get; set; } = [];
    }

}
