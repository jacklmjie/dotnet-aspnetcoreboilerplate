using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamicPlugins.Data
{
    /// <summary>
    /// LoadFromAssemblyPath加载程序集，程序集的文件位置被自动记录下来，但是我们改用LoadFromStream之后，
    /// 所需的文件位置信息丢失了，是一个空字符串，所以.NET Core在尝试加载视图的时候，
    /// 遇到空字符串，抛出了一个非法路径的错误。
    /// </summary>
    public class MyAssemblyPart : AssemblyPart, ICompilationReferencesProvider
    {
        public MyAssemblyPart(Assembly assembly) : base(assembly) { }

        public IEnumerable<string> GetReferencePaths() => Array.Empty<string>();
    }
}
