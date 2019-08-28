using Core.Common;
using Core.Entity;
using Core.IRepository;
using Core.Repository.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DapperDBContext _context;
        public StudentRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<int?> Add(Student entity)
        {
            return await _context.InsertAsync(entity);
        }

        public async Task<bool> Delete(Student entity)
        {
            var id = await _context.DeleteAsync(entity);
            return id > 0;
        }

        public async Task<bool> Update(Student entity)
        {
            var id = await _context.UpdateAsync(entity);
            return id > 0;
        }

        public async Task<Student> Get(long Id)
        {
            return await _context.GetAsync<Student>(Id);
        }

        public async Task<int> GetCount(QueryRequestByPage reqMsg)
        {
            return await _context.RecordCountAsync<Student>("where Name = @Name",
                    new { Name = reqMsg.Keyword });
        }

        public async Task<IEnumerable<Student>> GetListPaged(QueryRequestByPage reqMsg)
        {
            return await _context.GetListPagedAsync<Student>(reqMsg.PageIndex, reqMsg.PageSize,
                    "where Name = @Name", "Id desc",
                    new { Name = reqMsg.Keyword });
        }
    }
}
