using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace DemoDapper
{
    /// <summary>
    /// Exprssion反射
    /// 从结果反推代码，反推成DemoExtension2类似的直接读取类的代码
    /// </summary>
    public static class DemoExtensionExprssion_4
    {
        public static IEnumerable<T> Query4<T>(this IDbConnection cnn, string sql) where T : new()
        {
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    var func = CreateMappingFunction(reader, typeof(T));
                    while (reader.Read())
                    {
                        var result = func(reader as DbDataReader);
                        yield return result is T ? (T)result : default(T);
                    }

                }
            }
        }

        /// <summary>
        /// 反推成直接反射类的方法
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Func<DbDataReader, object> CreateMappingFunction(IDataReader reader, Type type)
        {
            //1. 取得sql select所有栏位名称
            var names = Enumerable.Range(0, reader.FieldCount).Select(index => reader.GetName(index)).ToArray();

            //2. 取得mapping类别的属性资料 >  将index,sql栏位,class属性资料做好对应封装在一个变量内方便后面使用
            var props = type.GetProperties().ToList();
            var members = names.Select((columnName, index) =>
            {
                var property = props.Find(p => string.Equals(p.Name, columnName, StringComparison.Ordinal))
                ?? props.Find(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));
                return new
                {
                    index,
                    columnName,
                    property
                };
            });

            //3. 动态建立方法 : 从数据库Reader按照顺序读取我们要的资料
            /*方法逻辑 : 
              User 动态方法(IDataReader reader)
              {
                var user = new User();
                var value = reader[0];
                if( !(value is System.DBNull) )
                  user.Name = (string)value;
                value = reader[1];
                if( !(value is System.DBNull) )
                  user.Age = (int)value;  
                return user;
              }
            */
            var exBodys = new List<Expression>();
            {
                // 方法(IDataReader reader)
                var exParam = Expression.Parameter(typeof(DbDataReader), "reader");

                // Mapping类别 物件 = new Mapping类别();
                var exVar = Expression.Variable(type, "mappingObj");
                var exNew = Expression.New(type);
                {
                    exBodys.Add(Expression.Assign(exVar, exNew));
                }

                // var value = defalut(object);
                var exValueVar = Expression.Variable(typeof(object), "value");
                {
                    exBodys.Add(Expression.Assign(exValueVar, Expression.Constant(null)));
                }


                var getItemMethod = typeof(DbDataReader).GetMethods().Where(w => w.Name == "get_Item")
                  .First(w => w.GetParameters().First().ParameterType == typeof(int));
                foreach (var m in members)
                {
                    //reader[0]
                    var exCall = Expression.Call(
                      exParam, getItemMethod,
                      Expression.Constant(m.index)
                    );

                    // value = reader[0];
                    exBodys.Add(Expression.Assign(exValueVar, exCall));

                    //user.Name = (string)value;
                    var exProp = Expression.Property(exVar, m.property.Name);
                    var exConvert = Expression.Convert(exValueVar, m.property.PropertyType); //(string)value
                    var exPropAssign = Expression.Assign(exProp, exConvert);

                    //if ( !(value is System.DBNull))
                    //    (string)value
                    var exIfThenElse = Expression.IfThen(
                      Expression.Not(Expression.TypeIs(exValueVar, typeof(System.DBNull)))
                      , exPropAssign
                    );

                    exBodys.Add(exIfThenElse);
                }


                // return user;  
                exBodys.Add(exVar);

                // Compiler Expression 
                var lambda = Expression.Lambda<Func<DbDataReader, object>>(
                  Expression.Block(
                    new[] { exVar, exValueVar },
                    exBodys
                  ), exParam
                );

                return lambda.Compile();
            }
        }
    }
}
