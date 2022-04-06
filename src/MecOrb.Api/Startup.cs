using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MecOrb.Api.Filters;
using MecOrb.Api.Logging;
using MecOrb.CrossCutting.Assemblies;
using MecOrb.CrossCutting.IoC;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MecOrb.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddMvc(options => options.Filters.Add(new DefaultExceptionFilterAttribute()));

            services.AddLoggingSerilog();

            services.AddAutoMapper(AssemblyUtil.GetCurrentAssemblies());

            services.AddDependencyResolver();

            services.AddSqlConnection(Configuration);

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MecOrb",
                    Description = "API - MecOrb",
                    Version = "v1"
                });

                var apiPath = Path.Combine(AppContext.BaseDirectory, "MecOrb.Api.xml");
                var applicationPath = Path.Combine(AppContext.BaseDirectory, "MecOrb.Application.xml");

                c.IncludeXmlComments(apiPath);
                c.IncludeXmlComments(applicationPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/MecOrb");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/MecOrb/swagger/v1/swagger.json", "API MecOrb");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
