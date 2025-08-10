using BookRadar.Common.DTOs;

namespace BookRadar.WebAPP.Models
{
    public class HistorialSearchViewModel
    {
        public string Autor { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / Math.Max(1, PageSize));
        public List<HistorialBusquedaDTO> Items { get; set; } = [];
    }
}
