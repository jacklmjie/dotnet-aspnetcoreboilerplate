using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DynamicPlugins.Infrastructure
{
    /// <summary>
    /// 所有插件都放在PluginsLoadContext对象里
    /// CollectibleAssemblyLoadContext创建了一个可回收的上下文
    /// </summary>
    public static class PluginsLoadContexts
    {
        private static Dictionary<string, CollectibleAssemblyLoadContext> _pluginContexts = null;

        static PluginsLoadContexts()
        {
            _pluginContexts = new Dictionary<string, CollectibleAssemblyLoadContext>();
        }

        public static bool Any(string pluginName)
        {
            return _pluginContexts.ContainsKey(pluginName);
        }

        public static void RemovePluginContext(string pluginName)
        {
            if (_pluginContexts.ContainsKey(pluginName))
            {
                _pluginContexts[pluginName].Unload();
                _pluginContexts.Remove(pluginName);
            }
        }

        public static CollectibleAssemblyLoadContext GetContext(string pluginName)
        {
            return _pluginContexts[pluginName];
        }

        public static void AddPluginContext(string pluginName, CollectibleAssemblyLoadContext context)
        {
            _pluginContexts.Add(pluginName, context);
        }
    }

    /// <summary>
    /// 一个可回收的程序集上下文
    /// </summary>
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        public CollectibleAssemblyLoadContext() : base(isCollectible: true)
        {
        }

        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }
}
