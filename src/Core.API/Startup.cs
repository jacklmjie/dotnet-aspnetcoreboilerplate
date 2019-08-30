using AutoMapper;
using Core.Common;
using Core.Common.Options;
using Core.Models.Identity.Entities;
using Core.Repository.Infrastructure;
using EasyCaching.Core;
using EasyCaching.Interceptor.AspectCore;
using EasyCaching.Redis;
using EasyCaching.Serialization.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using SmartSql.Starter.API.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

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
            //services.AddDapperDBContext<TestDBContext>(options =>
            //{
            //    options.Configuration = @"server=127.0.0.1;database=test;uid=root;pwd=123456;SslMode=none;";
            //});
            services.Configure<SwaggerOption>(Configuration.GetSection("Swagger"));
            services.Configure<JwtOption>(Configuration.GetSection("Jwt"));
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalValidateModelFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
            RegisterRepository(services);
            RegisterService(services);
            RegisterMapping(services);
            RegisterJwt(services);
            RegisterSwagger(services);
            RegisterEasyCaching(services);
        }

        private void RegisterRepository(IServiceCollection services)
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
                    services.AddTransient(p, type);
                }
            }
        }

        private void RegisterService(IServiceCollection services)
        {
            var assembly = Assembly.Load("Core.Contract");
            var allTypes = assembly.GetTypes().Where(t =>
            t.GetTypeInfo().IsClass &&
            !t.GetTypeInfo().IsAbstract &&
            t.GetTypeInfo().Name.EndsWith("Contract"));
            foreach (var type in allTypes)
            {
                var types = type.GetInterfaces();
                foreach (var p in types)
                {
                    services.AddTransient(p, type);
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

        private void RegisterJwt(IServiceCollection services)
        {
            // 替换 IPrincipal ，设置用户主键类型，用以在Repository进行审计时注入正确用户主键类型
            services.Replace(new ServiceDescriptor(typeof(IPrincipal),
                provider =>
                {
                    IHttpContextAccessor accessor = provider.GetService<IHttpContextAccessor>();
                    ClaimsPrincipal principal = accessor?.HttpContext?.User;
                    if (principal != null && principal.Identity is ClaimsIdentity identity)
                    {
                        PropertyInfo property = typeof(User).GetProperty("Id");
                        if (property != null)
                        {
                            identity.AddClaim(new Claim("userIdTypeName", property.PropertyType.FullName));
                        }
                    }

                    return principal;
                },
                ServiceLifetime.Transient));

            var jwtOption = Configuration.GetSection("Jwt").Get<JwtOption>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                string secret = jwtOption.Secret;
                if (string.IsNullOrEmpty(secret))
                {
                    throw new APIException("500", "配置文件中配置的Jwt节点的Secret不能为空");
                }

                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.Now,
                    ValidateLifetime = true
                };
            });
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            var swaggerOption = Configuration.GetSection("Swagger").Get<SwaggerOption>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"{swaggerOption.Version}", new Info() { Title = swaggerOption.Title, Version = $"{swaggerOption.Version}" });
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
                //权限Token
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    In = "header"
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", Enumerable.Empty<string>() }
                });
            });
        }

        private void RegisterEasyCaching(IServiceCollection services)
        {
            services.AddEasyCaching(option =>
            {
                option.UseRedis(Configuration, "redis2", "EasyCaching:redis").WithJson();
            });
            services.ConfigureAspectCoreInterceptor(options =>
            {
                options.CacheProviderName = "redis2";
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

            var swaggerOption = Configuration.GetSection("Swagger").Get<SwaggerOption>();
            if (!swaggerOption.Enabled)
            {
                return;
            }
            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", SERVICE_NAME);
            });
            app.UseAuthentication();
        }
    }
}
