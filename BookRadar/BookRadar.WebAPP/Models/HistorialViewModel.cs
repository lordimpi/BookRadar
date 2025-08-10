using BookRadar.Common.DTOs;

namespace BookRadar.WebAPP.Models
{
    public class HistorialViewModel
    {
        public List<HistorialBusquedaDTO> Items { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRows / PageSize);
    }
}
