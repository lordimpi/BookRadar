
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
            TypeAdapterConfig<HistorialBusqueda, HistorialBusquedaDTO>.NewConfig();
            TypeAdapterConfig<HistorialBusquedaDTO, HistorialBusqueda>.NewConfig();
        }
    }
}