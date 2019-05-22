using Core.Common;
using Core.Entity;
using System.Threading.Tasks;

namespace Core.IService
{
    public interface IStudentService
    {
        Task<bool> Add(Student entity);

        Task<bool> Delete(Student entity);

        Task<bool> Update(Student entity);

        Task<Student> Get(long Id);

        Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage reqMsg);
    }
}
