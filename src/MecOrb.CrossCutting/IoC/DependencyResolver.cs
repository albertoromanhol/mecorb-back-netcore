using Microsoft.Extensions.DependencyInjection;
using MecOrb.Application;
using MecOrb.Application.Interfaces;
using MecOrb.Domain.Repositories;
using MecOrb.Infrastructure.Repositories;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
            services.AddScoped<IDbConnection>(provider => {
                return new SqlConnection(configuration.GetSection("SQLConnection").Value);
            });
        }

        private static void RegisterApplications(IServiceCollection services)
        {
            services.AddScoped<IExemploApplication, ExemploApplication>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IExemploRepository, ExemploRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
        }
    }
}