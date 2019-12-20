using System.Collections.Generic;
using System.Data;

namespace DemoDapper
{
    /// <summary>
    /// Reflection反射
    /// </summary>
    public static class DemoExtensionReflection_3
    {
        public static IEnumerable<T> Query3<T>(this IDbConnection cnn, string sql) where T : new()
        {
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        yield return reader.CastToType<T>();
            }
        }

        /// <summary>
        /// Reflection反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        //1.使用泛型传递动态类别
        private static T CastToType<T>(this IDataReader reader) where T : new()
        {
            //2.使用泛型的条件约束new()达到动态建立物件
            var instance = new T();

            //3.DataReader需要使用属性字串名称当Key,可以使用Reflection取得动态类别的属性名称,在借由DataReader this[string parameter]取得数据库资料
            var type = typeof(T);
            var props = type.GetProperties();
            foreach (var p in props)
            {
                //缺点:
                //1.重复属性查询,没用到就要忽略
                //2.效率慢,reader[p.Name]比reader[index]效率慢
                //GetOrdinal 首先执行区分大小写的查找，参见文档
                //https://docs.microsoft.com/zh-cn/dotnet/api/system.data.sqlclient.sqldatareader.getordinal?redirectedfrom=MSDN&view=netframework-4.8
                var val = reader[p.Name];

                //4.使用PropertyInfo.SetValue方式动态将数据库资料赋予物件
                if (!(val is System.DBNull))
                    p.SetValue(instance, val);
            }
            return instance;
        }
    }
}
