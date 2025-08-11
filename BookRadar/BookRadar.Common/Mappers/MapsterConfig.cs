
using BookRadar.Common.DTOs;
using BookRadar.Common.Entities;
using Mapster;

namespace BookRadar.Common.Mappers
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            MapeoHistorialBusqueda();
        }

        private static void MapeoHistorialBusqueda()
        {
            TypeAdapterConfig<HistorialBusqueda, HistorialBusquedaDTO>
                .NewConfig()
                .Map(dest => dest.FechaConsultaUtc, src => src.FechaConsulta);

            TypeAdapterConfig<HistorialBusquedaDTO, HistorialBusqueda>
                .NewConfig()
                .Map(dest => dest.FechaConsulta, src => src.FechaConsultaUtc);
        }
    }
}