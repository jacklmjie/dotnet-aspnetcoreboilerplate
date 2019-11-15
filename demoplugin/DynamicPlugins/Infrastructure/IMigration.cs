using DynamicPlugins.Data;
using DynamicPlugins.ViewModels;
using System;

namespace DynamicPlugins.Infrastructure
{
    public interface IMigration
    {
        PluginVersion Version { get; }

        void MigrateUp(Guid pluginId);

        void MigrateDown(Guid pluginId);
    }
}
