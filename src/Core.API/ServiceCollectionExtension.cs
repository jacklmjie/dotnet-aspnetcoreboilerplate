using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.API
{
    /// <summary>
    /// 批量依赖注入扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注册多个程序集服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembyNames"></param>
        public static void RegisterService(this IServiceCollection services, Dictionary<string, string> assembyNames)
        {
            foreach (var assembyName in assembyNames)
            {
                services.RegisterService(assembyName.Key, assembyName.Value);
            }
        }

        /// <summary>
        /// 注册单个程序集服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembyName"></param>
        /// <param name="matchEnd"></param>
        public static void RegisterService(this IServiceCollection services, string assembyName, string matchEnd)
        {
            foreach (var item in GetClassName(assembyName, matchEnd))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
        }

        /// <summary>
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="assembyName"></param>
        /// <param name="matchEnd"></param>
        /// <returns></returns>
        private static Dictionary<Type, Type[]> GetClassName(string assembyName, string matchEnd)
        {
            var assembly = Assembly.Load(assembyName);
            var allTypes = assembly.GetTypes().Where(t =>
            t.GetTypeInfo().IsClass &&
            !t.GetTypeInfo().IsAbstract &&
            !t.GetTypeInfo().IsInterface &&
            t.GetTypeInfo().Name.EndsWith(matchEnd));

            var result = new Dictionary<Type, Type[]>();
            foreach (var item in allTypes)
            {
                var interfacesTypes = item.GetInterfaces();
                result.Add(item, interfacesTypes);
            }

            return result;
        }
    }
}
