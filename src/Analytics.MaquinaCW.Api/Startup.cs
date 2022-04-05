using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Analytics.MaquinaCW.Api.Filters;
using Analytics.MaquinaCW.Api.Logging;
using Analytics.MaquinaCW.CrossCutting.Assemblies;
using Analytics.MaquinaCW.CrossCutting.IoC;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Analytics.MaquinaCW.Api
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
                    Title = "Analytics.MaquinaCW",
                    Description = "API - Analytics.MaquinaCW",
                    Version = "v1"
                });

                var apiPath = Path.Combine(AppContext.BaseDirectory, "Analytics.MaquinaCW.Api.xml");
                var applicationPath = Path.Combine(AppContext.BaseDirectory, "Analytics.MaquinaCW.Application.xml");

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

            app.UsePathBase("/Analytics.MaquinaCW");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/Analytics.MaquinaCW/swagger/v1/swagger.json", "API Analytics.MaquinaCW");
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
