using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DemoDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //new EmitShow().EmitSayHello();

            var connectionString = "Server=.;Database=dapper;UID=sa;PWD=123456;";
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query6<User>("select N'jack' Name,27 Age").First();
                var result2 = connection.Query6<User>("select N'jack' Name,27 Age").First();
                Console.WriteLine(result.Name);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
