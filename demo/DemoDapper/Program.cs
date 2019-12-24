using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DemoDapper
{
    /// <summary>
    /// SplitOn区分类别Mapping组别
    /// TypeHandler 自订Mapping逻辑使用、底层逻辑
    /// QueryFirst。它使用CommandBehavior.SingleRow可以避免浪费资源只读取一行资料
    /// CreateParamInfoGenerator方法Emit IL建立AddParameter动态方法 > 建立完后保存在缓存内 > 忽略「没使用的栏位」
    /// DynamicParameter参数，字符串不指定长度，默认2000
    /// DynamicParameter预设AddParameters，加了许多判断，效率会比ADB.NET低，可以自己实现，例如CustomPraameters
    /// 简化了集合操作Execute,只需要 : connection.Execute("sql",集合参数),共用资源避免浪费(e.g共用同一个DbCommand、Func)
    /// 每跑一次对数据库送出一次reqesut,效率会被网路传输拖慢,所以被称为「多次执行」而不是「批量执行」的主要原因
    /// ExecuteScalar判断是否存在
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //new EmitShow().EmitSayHello();

            var connectionString = "Server=.;Database=dapper;UID=sa;PWD=123456;";
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<User>("select 1 Id,N'jack' Name,27 Age").First();
                var resultCatch = connection.Query<User>("select 1 Id,N'jack' Name,27 Age").First();
                //Query Multi Mapping
                var orderuser = connection.Query<Order, User, Order>(@"select * from [Order] t1
                                            left join [User] t2 on t2.Id = t1.UserId",
                                            (order, user) =>
                                            {
                                                order.User = user;
                                                return order;
                                            });
                //14.dynamic Multi Mapping
                var dynamic = connection.Query<dynamic, dynamic, dynamic>(@"select * from [Order] t1
                                            left join [User] t2 on t2.Id = t1.UserId",
                                           (post, user) =>
                                           {
                                               post.Owner = user;
                                               return post;
                                           });
                //SplitOn区分类别Mapping组别
                //假如主键名称是其他名称,请指定splitOn字串名称,并且对应多个可以使用,做区隔,举例,添加商品表格做Join 
                //从查询字段从后往前分割，分别赋值给不同的类，主键是Id可以省略
                var orderuserproduct = connection.Query<Order, User, Product, Order>(@"select * from [Order] t1
                                            left join [User] t2 on t2.Id = t1.UserId
                                            left join [Product] t3 on t3.Id=t1.ProductId",
                                            map: (order, user, product) =>
                                             {
                                                 order.User = user;
                                                 order.Product = product;
                                                 return order;
                                             },
                                            splitOn: "Id,Id");

                //15.使用QueryMultiple优点:
                //主要减少Reqeust次数
                //可以将多个查询共用同一组Parameter参数

                //QueryMultiple的底层实作逻辑:
                //底层技术是ADO.NET - DataReader - MultipleResult
                //QueryMultiple取得DataReader并封装进GridReader
                //呼叫Read方法时才会建立Mapping动态方法, Emit IL动作跟Query方法一样
                //接着使用ADO.NET技术呼叫DataReader NextResult取得下一组查询结果
                //假如没有下一组查询结果才会将DataReader释放
                using (var gridReader = connection.QueryMultiple("select 1; select 2;"))
                {
                    Console.WriteLine(gridReader.Read<int>()); //result : 1
                    Console.WriteLine(gridReader.Read<int>()); //result : 2
                }

                //16.TypeHandler 自订Mapping逻辑使用、底层逻辑
                //User资料变更时会自动在Log栏位纪录变更动作
                SqlMapper.AddTypeHandler(new JsonTypeHandler<List<Log>>());
                var user = new User()
                {
                    Name = "暐翰",
                    Age = 26,
                    Logs = new List<Log>() {
                        new Log()
                        {
                            Time=DateTime.Now,
                            Remark="CreateUser"
                        }
                    }
                };
                //新增用户
                connection.Execute("insert into [User] (Name,Age,Logs) values (@Name,@Age,@Logs);", user);

                var userlogs = connection.Query<User>("select * from [User]");
                Console.WriteLine(userlogs);

                //17.CommandBehavior的细节处理
                //会遇到select top 1知道只会读取一行资料的情况,这时候可以使用
                //QueryFirst。它使用CommandBehavior.SingleRow可以避免浪费资源只读取一行资料

                //与QuerySingle之间的差别
                //两者差别在QuerySingle没有使用CommandBehavior.SingleRow,至于为何没有使用,是因为需要有多行资料才能判断是否不符合条件并抛出Exception告知使用者

                //18.Parameter 参数化底层原理GetCacheInfo检查是否缓存内有动态方法 > 假如没有缓存,使用CreateParamInfoGenerator方法Emit IL建立AddParameter动态方法 > 建立完后保存在缓存内 > 忽略「没使用的栏位」不生成对应IL代码,避免资源浪费情况
                //虽然传递Age参数,但是SQL字串没有用到,Dapper不会去生成该栏位的SetParameter动作IL

                //19.IN 多集合参数化底层原理
                //以下用sys.objects来查SQL Server的表格跟视图当追踪例子 :
                //var result = cn.Query(@"select * from sys.objects where type_desc In @type_descs", new { type_descs = new[] { "USER_TABLE", "VIEW" } });
                //Dapper会将SQL字串改成以下方式执行
                //select* from sys.objects where type_desc In(@type_descs1, @type_descs2)
                //--@type_descs1 = nvarchar(4000) - 'USER_TABLE'
                //-- @type_descs2 = nvarchar(4000) - 'VIEW'

                //20.DynamicParameter 底层原理、自订实作
                var paramter = new
                {
                    Name = "John",
                    Age = 25
                };
                //String型态Dapper会自动将转成数据库Nvarchar并且长度为4000的参数
                var dynamic1 = connection.Query("select @Name Name,@Age Age", paramter).First();
                //实际执行过程如下
                //exec sp_executesql N'select @Name Name,@Age Age',N'@Name nvarchar(4000),@Age int',@Name=N'John',@Age=25
                var paramters = new DynamicParameters();
                paramters.Add("Name", "John", DbType.AnsiString, size: 4);
                paramters.Add("Age", 25, DbType.Int32);
                var dynamic2 = connection.Query("select @Name Name,@Age Age", paramters).First();

                //预设的实作类别DynamicParameters判断比较多，效率会较慢
                var customPraameters = new CustomPraameters();
                paramters.Add("Name", "John", DbType.AnsiString, size: 4);
                paramters.Add("Age", 25, DbType.Int32);
                var dynamic3 = connection.Query("select @Name Name,@Age Age", paramters).First();

                //21. 单次、多次 Execute 底层原理
                //单次执行,来说Dapper Execute底层是ADO.NET的ExecuteNonQuery的封装,封装目的为了跟Dapper的Parameter、缓存功能搭配使用
                //多次执行,共用资源避免浪费(e.g共用同一个DbCommand、Func),但遇到大量执行追求效率需求情况,需要特别注意
                //此方法每跑一次对数据库送出一次reqesut,效率会被网路传输拖慢
                //所以这功能被称为「多次执行」而不是「批量执行」的主要原因
                using (var tx = connection.BeginTransaction())
                {
                    connection.Execute("create table #T (V int);", transaction: tx);
                    //简化了集合操作Execute之间的操作,简化了代码,只需要 : connection.Execute("sql",集合参数);
                    connection.Execute("insert into #T (V) values (@V)", Enumerable.Range(1, 10).Select(val => new { V = val }).ToArray(), transaction: tx);

                    var dynamics = connection.Query("select * from #T", transaction: tx);
                    Console.WriteLine(result);
                }

                //22.ExecuteScalar应用
                //ExecuteScalar因为其只能读取第一组结果、第一笔列、第一笔资料特性,「查询资料是否存在」还是有用的
                //Entity Framwork如何高效率判断资料是否存在?使用Any而不是Count() > 1
                //Any语法转换SQL使用EXISTS,它只在乎是否有没有资料,不用检查到每列,只需要其中一笔就有结果,所以效率快
                var existsUser = connection.Any("select top 1 1 from [User]");

                Console.WriteLine(result.Name);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
