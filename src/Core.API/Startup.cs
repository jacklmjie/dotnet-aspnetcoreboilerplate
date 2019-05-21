using Core.API.Db;
using Core.IRepository;
using Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSql.Starter.API.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Core.API
{
    public class Startup
    {
        const string SERVICE_NAME = "Core.API";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalValidateModelFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
            var dbContextOptions = new DbContextOptionsBuilder<EFCoreDbContext>()
                                   .UseSqlServer(Configuration["ConnectionStrings:EFCoreDbContext"]).Options;
            services.AddScoped(s => new EFCoreDbContext(dbContextOptions));
            services.AddScoped<IUnitOfWork, UnitOfWork<EFCoreDbContext>>();
            RegisterRepository(services);
            RegisterService(services);
            RegisterSwagger(services);
        }

        private void RegisterRepository(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.IService");
            var allTypes = assembly.GetTypes();
            foreach (var type in allTypes)
            {
                services.AddScoped(type);
            }
        }

        private void RegisterService(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.IRepository");
            var allTypes = assembly.GetTypes();
            foreach (var type in allTypes)
            {
                services.AddScoped(type);
            }
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = SERVICE_NAME,
                    Version = "v1",
                    Description = "https://github.com/jacklmjie/aspnetcoreboilerplate"
                });
                c.CustomSchemaIds((type) => type.FullName);
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{SERVICE_NAME}.xml");
                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            ConfigureSwagger(app);
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {

            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", SERVICE_NAME);
            });
        }
    }
}
