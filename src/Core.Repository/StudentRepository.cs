using Core.Entity;
using Core.IRepository;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _config;
        public StudentRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }

        public async Task<int> Add(Student entity)
        {
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var id = await conn.InsertAsync(entity);
                return id;
            }
        }
    }
}
