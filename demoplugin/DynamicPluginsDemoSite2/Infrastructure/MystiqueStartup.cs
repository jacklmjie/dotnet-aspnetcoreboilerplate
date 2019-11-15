using DynamicPlugins.Data;
using DynamicPlugins.Extensions;
using DynamicPlugins.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynamicPluginsDemoSite2.Infrastructure
{
    public static class MystiqueStartup
    {
        private static IList<string> _presets = new List<string>();

        public static void MystiqueSetup(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IMvcModuleSetup, MvcModuleSetup>();
            services.AddScoped<IPluginManager, PluginManager>();
            services.AddSingleton<IActionDescriptorChangeProvider>(ActionDescriptorChangeProvider.Instance);
            services.AddSingleton<IReferenceContainer, DefaultReferenceContainer>();
            services.AddSingleton<IReferenceLoader, DefaultReferenceLoader>();
            services.AddSingleton(ActionDescriptorChangeProvider.Instance);

            var mvcBuilder = services.AddMvc();

            var provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var pluginManager = scope.ServiceProvider.GetService<IPluginManager>();
                var allEnabledPlugins = pluginManager.GetAllEnabledPlugins();
                var loader = scope.ServiceProvider.GetService<IReferenceLoader>();

                foreach (var plugin in allEnabledPlugins)
                {
                    var context = new CollectibleAssemblyLoadContext();
                    var moduleName = plugin.Name;
                    var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{moduleName}\\{moduleName}.dll";
                    var referenceFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{moduleName}";

                    _presets.Add(filePath);
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        // LoadFromAssemblyPath改成了LoadFromStream
                        var assembly = context.LoadFromStream(fs);
                        loader.LoadStreamsIntoContext(context, referenceFolderPath, assembly);

                        var controllerAssemblyPart = new MyAssemblyPart(assembly);
                        mvcBuilder.PartManager.ApplicationParts.Add(controllerAssemblyPart);
                        PluginsLoadContexts.AddPluginContext(plugin.Name, context);
                    }
                }
            }

            //Razor视图的运行时编译
            mvcBuilder.AddRazorRuntimeCompilation(o =>
            {
                foreach (var item in _presets)
                {
                    o.AdditionalReferencePaths.Add(item);
                }

                MyAdditionalReferencePathHolder.AdditionalReferencePaths = o.AdditionalReferencePaths;
            });

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.AreaViewLocationFormats.Add("/Modules/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });
        }
    }
}
