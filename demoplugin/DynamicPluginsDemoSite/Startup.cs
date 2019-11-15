using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DynamicPluginsDemoSite.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicPluginsDemoSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            ////动态加载插件中的控制器
            ////https://www.cnblogs.com/lwqlun/p/11137788.html#4310745
            //var assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "DemoPlugin1.dll");
            //var assemblyView = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "DemoPlugin1.Views.dll");
            //var viewAssemblyPart = new CompiledRazorAssemblyPart(assemblyView);

            //var controllerAssemblyPart = new AssemblyPart(assembly);
            //var mvcBuilders = services.AddMvc();

            //mvcBuilders.ConfigureApplicationPartManager(apm =>
            //{
            //    apm.ApplicationParts.Add(controllerAssemblyPart);
            //    apm.ApplicationParts.Add(viewAssemblyPart);
            //});

            //mvcBuilders.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IActionDescriptorChangeProvider>(MyActionDescriptorChangeProvider.Instance);
            services.AddSingleton(MyActionDescriptorChangeProvider.Instance);

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.AreaViewLocationFormats.Add("/Modules/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                name: "default",
                template: "Modules/{area}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
