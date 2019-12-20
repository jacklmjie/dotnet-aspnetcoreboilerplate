using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DemoDapper
{
    /// <summary>
    /// 缓存
    /// sql:区分不同SQL字串 (使用不当会导致效率慢、内存泄漏,缓存sql主要是为了,查询栏位顺序)
    /// 优化
    /// 1.参数化,select @guid只会一个缓存,select {guid}会很多缓存，会造成内存泄漏
    /// 2.正确字串拼接方式 : Literal Replacement,{=VipLevel} VipLevel
    /// GetCacheInfo下GetLiteralTokens在建立缓存之前会抓取SQL字串内符合{=变量名称}规格的资料
    /// </summary>
    public static class DemoExtensionCatch_6
    {
        private static readonly Dictionary<DapperIdentity, Func<DbDataReader, object>> readers = new Dictionary<DapperIdentity, Func<DbDataReader, object>>();

        public static IEnumerable<T> Query6<T>(this IDbConnection cnn, string sql, object param = null) where T : new()
        {
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                //dapper会在取缓存之前先正则匹配={变量名称}规格的替换
                //Dapper Literal Replacements底层原理就是字串取代
                CommandLiteralReplace(command, param);
                using (var reader = command.ExecuteReader())
                {
                    var identity = new DapperIdentity(command.CommandText, command.CommandType, cnn.ConnectionString, typeof(T), param?.GetType());

                    // 2. 如果cache有资料就使用,没有资料就动态建立方法并保存在缓存内
                    if (!readers.TryGetValue(identity, out Func<DbDataReader, object> func))
                    {
                        //动态建立方法
                        func = GetTypeDeserializerImpl(typeof(T), reader);
                        readers[identity] = func;
                        Console.WriteLine("没有缓存,建立动态方法放进缓存");
                    }
                    else
                    {
                        Console.WriteLine("使用缓存");
                    }


                    // 3. 呼叫生成的方法by reader,读取资料回传
                    while (reader.Read())
                    {
                        var result = func(reader as DbDataReader);
                        yield return result is T ? (T)result : default(T);
                    }
                }

            }
        }

        /// <summary>
        /// Dapper Literal Replacements
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        private static void CommandLiteralReplace(IDbCommand cmd, object param)
        {
            //dapper里18是取param的参数
            cmd.CommandText = cmd.CommandText.Replace("{=Age}", "18");
        }


        private static Func<DbDataReader, object> GetTypeDeserializerImpl(Type type, IDataReader reader, int startBound = 0, int length = -1, bool returnNullIfFirstMissing = false)
        {
            //..略
            return DemoExtensionEmit_5.GetTypeDeserializerImpl(type, reader);
        }
    }
}
