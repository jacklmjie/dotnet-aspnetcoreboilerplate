using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicPlugins.Data;
using DynamicPlugins.Models;
using DynamicPlugins.ViewModels;

namespace DynamicPlugins.Infrastructure
{
    public abstract class BaseMigration : IMigration
    {
        private MyContext _myContext;
        public BaseMigration(MyContext myContext,
            PluginVersion version)
        {
            _myContext = myContext;
            Version = version;
        }

        public PluginVersion Version { get; }

        public abstract string UpScripts { get; }

        public abstract string DownScripts { get; }

        public void MigrateUp(System.Guid pluginId)
        {
            _myContext.PluginMigrations.FromSql(UpScripts);
            var model = new PluginMigration()
            {
                PluginMigrationId = System.Guid.NewGuid(),
                PluginId = pluginId,
                Version = Version.VersionNumber,
                Up = UpScripts,
                Down = DownScripts
            };
            _myContext.PluginMigrations.Add(model);
        }

        public void MigrateDown(System.Guid pluginId)
        {
            _myContext.PluginMigrations.FromSql(DownScripts);
            var model = _myContext.PluginMigrations.
                SingleOrDefault(x => x.PluginId.Equals(pluginId) && x.Version.Equals(Version.VersionNumber));
            if (model != null)
            {
                _myContext.PluginMigrations.Remove(model);
            }
        }
    }
}
