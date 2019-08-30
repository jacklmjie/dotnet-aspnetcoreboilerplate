using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Core.Repository.Infrastructure
{
    public class TestDBContext : DapperDBContext
    {
        public TestDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        protected override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
    }
}
