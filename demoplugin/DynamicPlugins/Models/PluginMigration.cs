using System;

namespace DynamicPlugins.Models
{
    /// <summary>
    /// 插件版本表
    /// </summary>
    public class PluginMigration
    {
        public Guid PluginMigrationId { get; set; }

        public Guid PluginId { get; set; }

        public string Version { get; set; }

        public string Up { get; set; }

        public string Down { get; set; }
    }
}
