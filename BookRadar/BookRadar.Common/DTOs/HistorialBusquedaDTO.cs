namespace BookRadar.Common.DTOs
{
    public class HistorialBusquedaDTO
    {
        public string Autor { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public int? AnioPublicacion { get; set; }

        public string Editorial { get; set; } = string.Empty;

        public DateTime FechaConsultaUtc { get; set; }
    }
}