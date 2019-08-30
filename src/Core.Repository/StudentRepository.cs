using Core.Common;
using Core.IRepository;
using Core.Models.Identity.Entity;
using Core.Repository.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IRepository<Student, long> _repository;
        public StudentRepository(IRepository<Student, long> repository)
        {
            _repository = repository;
        }

        public async Task<long> Add(Student entity)
        {
            return await _repository.InsertAsync(entity);
        }

        public async Task<bool> Delete(Student entity)
        {
            return await _repository.DeleteAsync(entity) > 0;
        }

        public async Task<bool> Update(Student entity)
        {
            return await _repository.UpdateAsync(entity) > 0;
        }

        public async Task<Student> Get(long Id)
        {
            return await _repository.GetAsync(Id);
        }

        public async Task<int> GetCount(QueryRequestByPage dto)
        {
            return await _repository.RecordCountAsync();
        }

        public async Task<IEnumerable<Student>> GetListPaged(QueryRequestByPage dto)
        {
            return await _repository.GetListPagedAsync(dto.PageIndex, dto.PageSize,
                    "", "Id desc");
        }
    }
}
