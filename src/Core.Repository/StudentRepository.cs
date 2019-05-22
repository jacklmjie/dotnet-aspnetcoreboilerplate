using Core.Entity;
using Core.IRepository;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDbRepository _dbRepository;
        public StudentRepository(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<int> Add(Student entity)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                var id = await conn.InsertAsync(entity);
                return id;
            }
        }
    }
}
