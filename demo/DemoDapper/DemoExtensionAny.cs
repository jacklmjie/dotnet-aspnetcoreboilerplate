using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DemoDapper
{
    /// <summary>
    /// ExecuteScalar因为其只能读取第一组结果、第一笔列、第一笔资料特性,「查询资料是否存在」还是有用的
    /// Any语法转换SQL使用EXISTS,它只在乎是否有没有资料,不用检查到每列,只需要其中一笔就有结果,所以效率快
    /// </summary>
    public static class DemoExtensionAny
    {
        public static bool Any(this IDbConnection cn, string sql, object paramter = null)
        {
            //不要QueryFirstOrDefault,因为QueryFirstOrDefault判断结果为null时直接强转型
            return cn.ExecuteScalar<bool>(sql, paramter);
        }
    }
}
