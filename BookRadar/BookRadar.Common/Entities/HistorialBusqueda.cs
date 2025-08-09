namespace BookRadar.Common.Entities
{
    public class HistorialBusqueda
    {
        public int Id { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int? AnioPublicacion { get; set; }
        public string Editorial { get; set; } = string.Empty;
        public DateTime FechaConsulta { get; set; }
    }
}