using Core.Common;
using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IStudentRepository
    {
        Task<bool> Add(Student entity);
        Task<bool> Delete(Student entity);
        Task<bool> Update(Student entity);
        Task<Student> Get(long Id);
        Task<int> GetCount(QueryRequestByPage reqMsg);
        Task<IEnumerable<Student>> GetListPaged(QueryRequestByPage reqMsg);
    }
}
