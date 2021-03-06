using AutoMapper;
using MecOrb.Api.Filters;
using MecOrb.Api.Logging;
using MecOrb.CrossCutting.Assemblies;
using MecOrb.CrossCutting.IoC;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MecOrb.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {

        private readonly string CorsPolicy = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                builder =>
                {
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyOrigin()
                    .WithHeaders("Content-Type",
                                "X-Requested-With",
                                "Authorization",
                                "Access-Control-Allow-Headers",
                                "Access-Control-Request-Headers",
                                "Access-Control-Request-Method")
                    .AllowAnyMethod();
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddMvc(options => options.Filters.Add(new DefaultExceptionFilterAttribute()));

            services.AddApplicationInsightsTelemetry(opt => opt.EnableActiveTelemetryConfigurationSetup = true);

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

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(CorsPolicy);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRequestBodyLogging();
            //app.UseResponseBodyLogging();

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
