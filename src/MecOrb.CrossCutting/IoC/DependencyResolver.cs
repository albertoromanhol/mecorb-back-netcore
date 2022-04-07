using MecOrb.Application;
using MecOrb.Application.Interfaces;
using MecOrb.Domain.Repositories;
using MecOrb.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace MecOrb.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services)
        {
            RegisterApplications(services);
            RegisterRepositories(services);
        }

        public static void AddSqlConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(provider =>
            {
                return new SqlConnection(configuration.GetSection("SQLConnection").Value);
            });
        }

        private static void RegisterApplications(IServiceCollection services)
        {
            services.AddScoped<IPlanetApplication, PlanetApplication>();
            services.AddScoped<ISimulationApplication, SimulationApplication>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IPlanetRepository, PlanetRepository>();
            services.AddScoped<INasaHorizonRepository, NasaHorizonRepository>();
        }
    }
}