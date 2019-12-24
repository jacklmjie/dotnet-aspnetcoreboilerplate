using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DemoDapper
{
    /// <summary>
    /// Dapper预设的实作类别DynamicParameters中AddParameters方法的实作逻辑
    /// 为了方便性跟兼容其他功能,像是Literal Replacement、EnumerableMultiParameter功能,做了许多判断跟动作
    /// 所以代码量会比以前使用ADO.NET版本多,所以效率也会比较慢
    /// </summary>
    public class CustomPraameters : SqlMapper.IDynamicParameters
    {
        private SqlParameter[] parameters;
        public void Add(params SqlParameter[] mParameters)
        {
            parameters = mParameters;
        }

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            if (parameters != null && parameters.Length > 0)
                foreach (var p in parameters)
                    command.Parameters.Add(p);
        }
    }
}
