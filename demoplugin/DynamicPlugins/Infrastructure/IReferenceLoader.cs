using DynamicPlugins.Extensions;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace DynamicPlugins.Infrastructure
{
    /// <summary>
    /// 通过AssemblyLoadContext加载程序集的时候
    /// 我们只加载了插件程序集，没有加载它引用的程序集
    /// </summary>
    public interface IReferenceLoader
    {
        public void LoadStreamsIntoContext(CollectibleAssemblyLoadContext context, string moduleFolder,
            Assembly assembly);
    }

    public class DefaultReferenceLoader : IReferenceLoader
    {
        private IReferenceContainer _referenceContainer = null;
        private readonly ILogger<DefaultReferenceLoader> _logger = null;

        public DefaultReferenceLoader(IReferenceContainer referenceContainer, ILogger<DefaultReferenceLoader> logger)
        {
            _referenceContainer = referenceContainer;
            _logger = logger;
        }

        public void LoadStreamsIntoContext(CollectibleAssemblyLoadContext context, string moduleFolder, Assembly assembly)
        {
            var references = assembly.GetReferencedAssemblies();

            foreach (var item in references)
            {
                var name = item.Name;

                var version = item.Version.ToString();

                var stream = _referenceContainer.GetStream(name, version);

                if (stream != null)
                {
                    _logger.LogDebug($"Found the cached reference '{name}' v.{version}");
                    context.LoadFromStream(stream);
                }
                else
                {

                    if (IsSharedFreamwork(name))
                    {
                        continue;
                    }

                    var dllName = $"{name}.dll";
                    var filePath = $"{moduleFolder}\\{dllName}";

                    if (!File.Exists(filePath))
                    {
                        _logger.LogWarning($"The package '{dllName}' is missing.");
                        continue;
                    }

                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        var referenceAssembly = context.LoadFromStream(fs);

                        var memoryStream = new MemoryStream();

                        fs.Position = 0;
                        fs.CopyTo(memoryStream);
                        fs.Position = 0;
                        memoryStream.Position = 0;
                        _referenceContainer.SaveStream(name, version, memoryStream);

                        LoadStreamsIntoContext(context, moduleFolder, referenceAssembly);
                    }
                }
            }
        }

        private bool IsSharedFreamwork(string name)
        {
            return SharedFrameworkConst.SharedFrameworkDLLs.Contains($"{name}.dll");
        }
    }
}
