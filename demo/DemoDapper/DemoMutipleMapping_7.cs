using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DemoDapper
{
    /// <summary>
    /// 多映射原理
    /// 1.按照泛型类别参数数量建立对应数量的Mapping Func集合
    /// 2.Mapping Func建立逻辑跟Query Emit IL一样
    /// 3.呼叫使用者的Custom Mapping Func,其中参数由前面动态生成的Mapping Func而来
    /// </summary>
    public static class DemoMutipleMapping_7
    {
        public static IEnumerable<TReturn> Query7<T1, T2, TReturn>(this IDbConnection connection, string sql, Func<T1, T2, TReturn> map)
    where T1 : Order, new()
    where T2 : User, new() //这两段where单纯为了Demo方便
        {
            //1. 按照泛型类别参数数量建立对应数量的Mapping Func集合
            var deserializers = new List<Func<IDataReader, object>>();
            {
                //2. Mapping Func建立逻辑跟Query Emit IL一样
                deserializers.Add((reader) =>
                {
                    var newObj = new T1();
                    var value = default(object);
                    value = reader[0];
                    newObj.Id = value is DBNull ? 0 : (int)value;
                    value = reader[1];
                    newObj.OrderNo = value is DBNull ? null : (string)value;
                    return newObj;
                });

                deserializers.Add((reader) =>
                {
                    var newObj = new T2();
                    var value = default(object);
                    value = reader[2];
                    newObj.Id = value is DBNull ? 0 : (int)value;
                    value = reader[4];
                    newObj.Name = value is DBNull ? null : (string)value;
                    return newObj;
                });
            }


            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //3. 呼叫使用者的Custom Mapping Func,其中参数由前面动态生成的Mapping Func而来
                        yield return map(deserializers[0](reader) as T1, deserializers[1](reader) as T2);
                    }
                }
            }
        }
    }
}
