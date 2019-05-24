using AutoMapper;
using Core.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSql.Starter.API.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
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
            RegisterRepository(services);
            RegisterService(services);
            RegisterMapping(services);
            RegisterSwagger(services);
        }

        private void RegisterRepository(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.Service");
            var allTypes = assembly.GetTypes().Where(t =>
            t.GetTypeInfo().IsClass &&
            !t.GetTypeInfo().IsAbstract &&
            t.GetTypeInfo().Name.EndsWith("Service"));
            foreach (var type in allTypes)
            {
                var types = type.GetInterfaces();
                foreach (var p in types)
                {
                    services.AddScoped(p, type);
                }
            }
        }

        private void RegisterService(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.Repository");
            var allTypes = assembly.GetTypes().Where(t =>
            t.GetTypeInfo().IsClass &&
            !t.GetTypeInfo().IsAbstract &&
            t.GetTypeInfo().Name.EndsWith("Repository"));
            foreach (var type in allTypes)
            {
                var types = type.GetInterfaces();
                foreach (var p in types)
                {
                    services.AddScoped(p, type);
                }
            }
        }

        private void RegisterMapping(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.Mapper");
            var allTypes = assembly.GetTypes().Where(t =>
            t.GetTypeInfo().IsClass &&
            !t.GetTypeInfo().IsAbstract &&
            t.GetTypeInfo().Name.EndsWith("Profile"));
            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                foreach (var type in allTypes)
                {
                    cfg.AddProfile(type);
                }
            });
            services.AddSingleton(config);
            services.AddScoped<IMapper, AutoMapper.Mapper>();
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
