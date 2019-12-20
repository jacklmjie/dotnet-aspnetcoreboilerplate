using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace DemoDapper
{
    /// <summary>
    /// Dynamic Query 原理
    /// </summary>
    public static class DemoExtensionDynamic_1
    {
        public static IEnumerable<dynamic> Query1(this IDbConnection cnn, string sql)
        {
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader.CastToDynamic();
                    }
                }
            }
        }

        /// <summary>
        /// dynamic动态映射
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static dynamic CastToDynamic(this IDataReader reader)
        {
            //ExpandoObject有实现IDynamicMetaObjectProvider
            dynamic e = new ExpandoObject();
            var d = e as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
                d.Add(reader.GetName(i), reader[i]);
            return e;
        }
    }
}
