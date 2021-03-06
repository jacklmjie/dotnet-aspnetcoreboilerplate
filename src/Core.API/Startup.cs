﻿using AutoMapper;
using Core.API.Filters;
using Core.Common.Options;
using Core.Models.Identity.Entities;
using Core.Repository.Infrastructure;
using EasyCaching.Interceptor.AspectCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers()//是否首行缩进，有很多配置
                    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            //services.AddControllers().AddNewtonsoftJson();
            services.AddDapperDBContext<TestDBContext>(options =>
            {
                options.Configuration = Configuration.GetConnectionString("MySqlCoreAPI");
            });
            services.Configure<SwaggerOption>(Configuration.GetSection("Swagger"));
            services.Configure<JwtOption>(Configuration.GetSection("Jwt"));
            services.AddMvc(options =>
                    {
                        options.Filters.Add<OperationCancelledExceptionFilter>();
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .ConfigureApiBehaviorOptions(options =>
                     {
                         //是否禁用multipart/form-data推断
                         options.SuppressConsumesConstraintForFormFileParameters = true;
                         //是否禁用绑定源推理
                         options.SuppressInferBindingSourcesForParameters = false;
                         //是否禁用 400自动验证
                         options.SuppressModelStateInvalidFilter = false;
                         //是否禁用 ProblemDetails 的自动创建
                         //比如NotFound 的 HTTP 响应具有 404 状态代码和 ProblemDetails 正文
                         options.SuppressMapClientErrors = true;
                         //400 响应的默认响应类型为 ValidationProblemDetails，更改为true改为 SerializableError
                         //todo:3.1冲突了 需验证
                         //options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = false;
                         options.SuppressInferBindingSourcesForParameters = false;
                     });
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
                    services.AddScoped(p, type);
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
                        PropertyInfo property = typeof(IdentityUser).GetProperty("Id");
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
                    throw new Exception("配置文件中配置的Jwt节点的Secret不能为空");
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
                options.SwaggerDoc($"{swaggerOption.Version}", new OpenApiInfo() { Title = swaggerOption.Title, Version = $"{swaggerOption.Version}" });
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
                //权限Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"}
                           },new string[] { }
                        }
                    });
            });
        }

        private void RegisterEasyCaching(IServiceCollection services)
        {
            services.AddEasyCaching(option =>
            {
                option.UseRedis(Configuration, "redis", "EasyCaching:redis").WithJson();
            });
            services.ConfigureAspectCoreInterceptor(options =>
            {
                options.CacheProviderName = "redis";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureSwagger(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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
        }
    }
}
