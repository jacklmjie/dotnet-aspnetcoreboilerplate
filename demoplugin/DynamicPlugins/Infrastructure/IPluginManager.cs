using DynamicPlugins.Data;
using DynamicPlugins.Models;
using DynamicPlugins.ViewModels;
using System;
using System.Collections.Generic;

namespace DynamicPlugins.Infrastructure
{
    public interface IPluginManager
    {
        List<Plugin> GetAllPlugins();

        Plugin GetPlugin(Guid pluginId);

        void AddPlugins(PluginPackage pluginPackage);

        void DeletePlugin(Guid pluginId);

        void EnablePlugin(Guid pluginId);

        void DisablePlugin(Guid pluginId);
    }
}
