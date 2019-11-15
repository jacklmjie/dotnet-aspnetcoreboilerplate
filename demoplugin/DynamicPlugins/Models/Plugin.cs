using System;

namespace DynamicPlugins.Models
{
    /// <summary>
    /// 插件信息表
    /// </summary>
    public class Plugin
    {
        public Guid PluginId { get; set; }

        public string UniqueKey { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Version { get; set; }

        public bool IsEnable { get; set; }
    }
}
