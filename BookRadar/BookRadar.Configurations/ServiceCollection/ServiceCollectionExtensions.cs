using BookRadar.Bussiness.Service.Http;
using BookRadar.Bussiness.Service.OpenLibrary;
using BookRadar.Common.Configurations;
using BookRadar.Common.IOptionPattern;
using BookRadar.Common.Mappers;
using BookRadar.DataAccess.Data;
using BookRadar.DataAccess.Repositories.Historial;
using BookRadar.DataAccess.UnitOfWork;
using BookRadar.DataAccess.Utilities.ADO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BookRadar.Configurations.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra el servicio genérico de opciones para cualquier T.
        /// </summary>
        public static IServiceCollection AddBookRadarOptionsCore(this IServiceCollection services)
        {
            // Habilita soporte para IOptions
            services.AddOptions();

            // Registrar DbContext usando IGenericOptionsService
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var connOptions = sp.GetRequiredService<IGenericOptionsService<DbOptions>>().GetSnapshotOptions();
                options.UseSqlServer(connOptions.DefaultConnection);
            });

            // Registra mapeos de Mapster
            MapsterConfig.RegisterMappings();
            
            services.AddHttpClient<IHttpService, HttpService>();

            services.AddScoped(typeof(IGenericOptionsService<>), typeof(GenericOptionsService<>));
            services.AddScoped<IOpenLibraryService, OpenLibraryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IHistorialRepository, HistorialRepository>();
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ISqlExecutor, SqlExecutor>();

            return services;
        }

        /// <summary>
        /// Vincula una clase de configuración TOptions a una sección del IConfiguration.
        /// Incluye validaciones opcionales con DataAnnotations y chequeo en inicio.
        /// </summary>
        public static OptionsBuilder<TOptions> BindOptions<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName,
            bool validateDataAnnotations = true,
            bool validateOnStart = true)
            where TOptions : class, new()
        {
            var builder = services
                .AddOptions<TOptions>()
                .Bind(configuration.GetSection(sectionName));

            if (validateDataAnnotations)
                builder.ValidateDataAnnotations();

            if (validateOnStart)
                builder.ValidateOnStart();

            return builder;
        }
    }
}