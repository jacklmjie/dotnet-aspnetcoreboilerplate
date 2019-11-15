using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DynamicPlugins.Extensions
{
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
