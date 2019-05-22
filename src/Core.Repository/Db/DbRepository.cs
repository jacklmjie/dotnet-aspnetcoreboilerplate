using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Repository
{
    public class DbRepository : IDbRepository, IDisposable
    {
        private readonly IConfiguration _config;
        public DbRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                IDbConnection conn = new SqlConnection(_config.GetConnectionString("MyConnectionString"));
                conn.Open();
                return conn;
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
