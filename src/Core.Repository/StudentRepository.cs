using Core.Common;
using Core.Entity;
using Core.IRepository;
using Dapper;
using System.Collections.Generic;
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

        public async Task<bool> Add(Student entity)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                var id = await conn.InsertAsync(entity);
                return id.HasValue ? id > 0 ? true : false : true;
            }
        }

        public async Task<bool> Delete(Student entity)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                var id = await conn.DeleteAsync(entity);
                return id > 0;
            }
        }

        public async Task<bool> Update(Student entity)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                var id = await conn.UpdateAsync(entity);
                return id > 0;
            }
        }

        public async Task<Student> Get(long Id)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                return await conn.GetAsync<Student>(Id);
            }
        }

        public async Task<int> GetCount(QueryRequestByPage reqMsg)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                return await conn.RecordCountAsync<Student>("where Name = @Name",
                    new { Name = reqMsg.Keyword });
            }
        }

        public async Task<IEnumerable<Student>> GetListPaged(QueryRequestByPage reqMsg)
        {
            using (IDbConnection conn = _dbRepository.Connection)
            {
                return await conn.GetListPagedAsync<Student>(reqMsg.PageIndex, reqMsg.PageSize,
                    "where Name = @Name", "Id desc",
                    new { Name = reqMsg.Keyword });
            }
        }
    }
}
