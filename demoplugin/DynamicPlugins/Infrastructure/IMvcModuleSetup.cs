using DynamicPlugins.Data;
using DynamicPlugins.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.IO;
using System.Linq;

namespace DynamicPlugins.Infrastructure
{
    public interface IMvcModuleSetup
    {
        void DisableModule(string moduleName);

        void EnableModule(string moduleName);

        void DeleteModule(string moduleName);
    }

    public class MvcModuleSetup : IMvcModuleSetup
    {
        private ApplicationPartManager _partManager;
        private IReferenceLoader _referenceLoader = null;

        public MvcModuleSetup(ApplicationPartManager partManager, IReferenceLoader referenceLoader)
        {
            _partManager = partManager;
            _referenceLoader = referenceLoader;
        }

        public void EnableModule(string moduleName)
        {
            if (!PluginsLoadContexts.Any(moduleName))
            {
                var context = new CollectibleAssemblyLoadContext();

                var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{moduleName}\\{moduleName}.dll";
                var referenceFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{moduleName}";
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var assembly = context.LoadFromStream(fs);
                    _referenceLoader.LoadStreamsIntoContext(context, referenceFolderPath, assembly);

                    var controllerAssemblyPart = new MyAssemblyPart(assembly);

                    MyAdditionalReferencePathHolder.AdditionalReferencePaths.Add(filePath);
                    _partManager.ApplicationParts.Add(controllerAssemblyPart);
                    PluginsLoadContexts.AddPluginContext(moduleName, context);
                }
            }
            else
            {
                var context = PluginsLoadContexts.GetContext(moduleName);
                var controllerAssemblyPart = new MyAssemblyPart(context.Assemblies.First());
                _partManager.ApplicationParts.Add(controllerAssemblyPart);
            }

            ResetControllActions();
        }

        public void DisableModule(string moduleName)
        {
            var last = _partManager.ApplicationParts.First(p => p.Name == moduleName);
            _partManager.ApplicationParts.Remove(last);

            ResetControllActions();
        }

        public void DeleteModule(string moduleName)
        {
            PluginsLoadContexts.RemovePluginContext(moduleName);

            var directory = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{moduleName}");
            directory.Delete(true);
        }

        private void ResetControllActions()
        {
            ActionDescriptorChangeProvider.Instance.HasChanged = true;
            ActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
        }
    }
}
