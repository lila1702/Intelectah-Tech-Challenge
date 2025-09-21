using Microsoft.Extensions.DependencyInjection;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Interfaces.External;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Infrastructure.Data;
using CarDealershipManager.Infrastructure.Identity;
using CarDealershipManager.Infrastructure.Mapping;
using CarDealershipManager.Infrastructure.Repositories;
using CarDealershipManager.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Repositories
            services.AddScoped<IFabricanteRepository, FabricanteRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IConcessionariaRepository, ConcessionariaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();

            // Services
            services.AddScoped<IFabricanteService, FabricanteService>();
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IConcessionariaService, ConcessionariaService>();
            services.AddScoped<IVendaService, VendaService>();
            services.AddScoped<ICacheService, CacheService>();

            // External Services
            services.AddHttpClient<ICEPService, CEPService>();

            return services;
        }
    }
}
