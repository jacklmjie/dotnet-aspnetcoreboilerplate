using System.Collections.Generic;
using System.Data;

namespace DemoDapper
{
    /// <summary>
    /// ADO.NET读取
    /// </summary>
    public static class DemoExtensionADONET_2
    {
        public static IEnumerable<User> Query2<T>(this IDbConnection cnn, string sql)
        {
            if (cnn.State == ConnectionState.Closed) cnn.Open();
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        yield return reader.CastToUser();
            }
        }

        /// <summary>
        /// 直接读取类
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static User CastToUser(this IDataReader reader)
        {
            var user = new User();
            var value = reader[0];
            if (!(value is System.DBNull))
                user.Name = value.ToString();
            value = reader[1];
            if (!(value is System.DBNull))
                user.Age = (int)value;
            return user;
        }
    }
}
