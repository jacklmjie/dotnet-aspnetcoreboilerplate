using System;
using System.Data;

namespace DemoDapper
{
    /// <summary>
    /// dapper缓存类.简单实现
    /// </summary>
    public class DapperIdentity
    {
        /*Identity主要封装各缓存的比较Key属性 :
        sql : 区分不同SQL字串 (使用不当会导致效率慢、内存泄漏,缓存sql主要是为了,查询栏位顺序)
        type : 区分Mapping类别
        commandType : 负责区分不同数据库
        gridIndex : 主用用在QueryMultiple,后面讲解。
        connectionString : 主要区分同数据库厂商但是不同DB情况
        parametersType : 主要区分参数类别
        typeCount : 主要用在Multi Query多映射, 需要搭配override GetType方法,后面讲解*/
        public string sql { get; set; }
        public CommandType? commandType { get; set; }
        public string connectionString { get; set; }
        public Type type { get; set; }
        public Type parametersType { get; set; }
        public DapperIdentity(string sql, CommandType? commandType, string connectionString, Type type, Type parametersType)
        {
            this.sql = sql;
            this.commandType = commandType;
            this.connectionString = connectionString;
            this.type = type;
            this.parametersType = parametersType;
            unchecked
            {
                hashCode = 17; // we *know* we are using this in a dictionary, so pre-compute this
                hashCode = (hashCode * 23) + commandType.GetHashCode();
                hashCode = (hashCode * 23) + (sql?.GetHashCode() ?? 0);
                hashCode = (hashCode * 23) + (type?.GetHashCode() ?? 0);
                hashCode = (hashCode * 23) + (connectionString == null ? 0 : StringComparer.Ordinal.GetHashCode(connectionString));
                hashCode = (hashCode * 23) + (parametersType?.GetHashCode() ?? 0);
            }
        }

        public readonly int hashCode;
        public override int GetHashCode() => hashCode;

        public override bool Equals(object obj) => Equals(obj as DapperIdentity);
        public bool Equals(DapperIdentity other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            return type == other.type
              && sql == other.sql
              && commandType == other.commandType
              && StringComparer.Ordinal.Equals(connectionString, other.connectionString)
              && parametersType == other.parametersType;
        }
    }
}
