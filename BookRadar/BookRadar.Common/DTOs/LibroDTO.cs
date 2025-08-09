namespace BookRadar.Common.DTOs
{
    public class LibroDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public int? AnioPublicacion { get; set; }
        public string? Editorial { get; set; }
    }
}