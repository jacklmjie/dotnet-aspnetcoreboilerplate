using DynamicPlugins.Data;
using DynamicPlugins.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DynamicPlugins.Infrastructure
{
    /// <summary>
    /// 插件包的业务处理接口
    /// </summary>
    public interface IPluginManager
    {
        List<Plugin> GetAllPlugins();

        List<Plugin> GetAllEnabledPlugins();

        Plugin GetPlugin(Guid pluginId);

        void AddPlugins(PluginPackage pluginPackage);

        void DeletePlugin(Guid pluginId);

        void EnablePlugin(Guid pluginId);

        void DisablePlugin(Guid pluginId);
    }

    public class PluginManager : IPluginManager
    {
        private MyContext _myContext;
        private IMvcModuleSetup _mvcModuleSetup;

        public PluginManager(MyContext myContext,
            IMvcModuleSetup mvcModuleSetup)
        {
            _myContext = myContext;
            _mvcModuleSetup = mvcModuleSetup;
        }

        public List<Plugin> GetAllPlugins()
        {
            return _myContext.Plugins.ToList();
        }

        public List<Plugin> GetAllEnabledPlugins()
        {
            return _myContext.Plugins.Where(x=>x.IsEnable).ToList();
        }

        public Plugin GetPlugin(Guid pluginId)
        {
            return _myContext.Plugins.FirstOrDefault(x => x.PluginId.Equals(pluginId));
        }

        public void DeletePlugin(Guid pluginId)
        {
            var plugin = GetPlugin(pluginId);
            if (plugin != null)
            {
                if (plugin.IsEnable)
                    DisablePlugin(pluginId);

                var migrations = _myContext.PluginMigrations.
                    OrderByDescending(x => x.Version).
                    Where(x => x.PluginId.Equals(pluginId)).
                    ToList();
                foreach (var item in migrations)
                {
                    _myContext.PluginMigrations.FromSql(item.Down);
                }
                _myContext.PluginMigrations.RemoveRange(migrations);

                _myContext.Plugins.Remove(plugin);
                _mvcModuleSetup.DeleteModule(plugin.Name);
            }
        }

        public void EnablePlugin(Guid pluginId)
        {
            var plugin = GetPlugin(pluginId);
            if (plugin != null)
            {
                plugin.IsEnable = true;
                _myContext.Plugins.Update(plugin);
                _mvcModuleSetup.EnableModule(plugin.Name);
            }
        }

        public void DisablePlugin(Guid pluginId)
        {
            var plugin = GetPlugin(pluginId);
            if (plugin != null)
            {
                plugin.IsEnable = false;
                _myContext.Plugins.Update(plugin);
                _mvcModuleSetup.DisableModule(plugin.Name);
            }
        }

        public void AddPlugins(PluginPackage pluginPackage)
        {
            var existedPlugin = _myContext.Plugins.
                            FirstOrDefault(x => x.PluginId.Equals(pluginPackage.Configuration.Name));

            if (existedPlugin == null)
            {
                InitializePlugin(pluginPackage);
            }
            else if (new Version(pluginPackage.Configuration.Version) > new Version(existedPlugin.Version))
            {
                UpgradePlugin(pluginPackage, existedPlugin);
            }
            else if (new Version(pluginPackage.Configuration.Version) == new Version(existedPlugin.Version))
            {
                throw new Exception("The package version is same as the current plugin version.");
            }
            else
            {
                DegradePlugin(pluginPackage, existedPlugin);
            }
        }

        private void InitializePlugin(PluginPackage pluginPackage)
        {
            var plugin = new Plugin
            {
                Name = pluginPackage.Configuration.Name,
                DisplayName = pluginPackage.Configuration.DisplayName,
                PluginId = Guid.NewGuid(),
                UniqueKey = pluginPackage.Configuration.UniqueKey,
                Version = pluginPackage.Configuration.Version
            };

            _myContext.Plugins.Add(plugin);

            var versions = pluginPackage.GetAllMigrations(_myContext);

            foreach (var version in versions)
            {
                version.MigrateUp(plugin.PluginId);
            }

            pluginPackage.SetupFolder();
        }

        private void UpgradePlugin(PluginPackage pluginPackage, Plugin oldPlugin)
        {
            oldPlugin.Version = pluginPackage.Configuration.Version;
            _myContext.Plugins.Update(oldPlugin);

            var migrations = pluginPackage.GetAllMigrations(_myContext);

            var pendingMigrations = migrations.Where(p => p.Version > oldPlugin.Version);

            foreach (var migration in pendingMigrations)
            {
                migration.MigrateUp(oldPlugin.PluginId);
            }

            pluginPackage.SetupFolder();
        }

        private void DegradePlugin(PluginPackage pluginPackage, Plugin oldPlugin)
        {
            oldPlugin.Version = pluginPackage.Configuration.Version;
            _myContext.Plugins.Update(oldPlugin);
        }
    }
}
