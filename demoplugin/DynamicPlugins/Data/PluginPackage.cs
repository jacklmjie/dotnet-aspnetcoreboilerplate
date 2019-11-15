using System;
using System.IO;
using System.IO.Compression;
using ZipTool = System.IO.Compression.ZipArchive;
using System.Linq;
using Newtonsoft.Json;
using DynamicPlugins.Extensions;
using DynamicPlugins.ViewModels;
using System.Collections.Generic;
using DynamicPlugins.Infrastructure;

namespace DynamicPlugins.Data
{
    /// <summary>
    /// 封装了插件包的相关操作
    /// </summary>
    public class PluginPackage
    {
        private Stream _zipStream = null;
        private string _tempFolderName = string.Empty;
        private string _folderName = string.Empty;
        public PluginConfiguration Configuration { get; private set; } = null;

        public PluginPackage(Stream stream)
        {
            _zipStream = stream;
            Initialize(stream);
        }

        public void Initialize(Stream stream)
        {
            _zipStream = stream;
            _tempFolderName = $"{ AppDomain.CurrentDomain.BaseDirectory }{ Guid.NewGuid().ToString()}";
            ZipTool archive = new ZipTool(_zipStream, ZipArchiveMode.Read);

            archive.ExtractToDirectory(_tempFolderName);

            var folder = new DirectoryInfo(_tempFolderName);

            var files = folder.GetFiles();

            var configFile = files.SingleOrDefault(p => p.Name == "plugin.json");

            if (configFile == null)
            {
                throw new MissingConfigurationFileException();
            }
            else
            {
                using (var s = configFile.OpenRead())
                {
                    LoadConfiguration(s);
                }
            }
        }

        public List<IMigration> GetAllMigrations(MyContext myContext)
        {
            var context = new CollectibleAssemblyLoadContext();
            var assemblyPath = $"{_tempFolderName}/{Configuration.Name}.dll";

            using (var fs = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read))
            {
                var assembly = context.LoadFromStream(fs);
                var migrationTypes = assembly.ExportedTypes.Where(p => p.GetInterfaces().Contains(typeof(IMigration)));

                List<IMigration> migrations = new List<IMigration>();
                foreach (var migrationType in migrationTypes)
                {
                    var constructor = migrationType.GetConstructors().First(p => p.GetParameters().Count() == 1 && p.GetParameters()[0].ParameterType == typeof(MyContext));

                    migrations.Add((IMigration)constructor.Invoke(new object[] { myContext }));
                }

                context.Unload();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                return migrations.OrderBy(p => p.Version).ToList();
            }
        }

        public void SetupFolder()
        {
            ZipTool archive = new ZipTool(_zipStream, ZipArchiveMode.Read);
            _zipStream.Position = 0;
            _folderName = $"{AppDomain.CurrentDomain.BaseDirectory}Modules\\{Configuration.Name}";

            archive.ExtractToDirectory(_folderName, true);

            var folder = new DirectoryInfo(_tempFolderName);
            folder.Delete(true);
        }

        private void LoadConfiguration(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var content = sr.ReadToEnd();
                Configuration = JsonConvert.DeserializeObject<PluginConfiguration>(content);

                if (Configuration == null)
                {
                    throw new WrongFormatConfigurationException();
                }
            }
        }
    }
}
