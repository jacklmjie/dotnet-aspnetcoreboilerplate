using Core.Common;
using Core.Entity;
using Core.Models;
using System.Threading.Tasks;

namespace Core.IService
{
    public interface IStudentService
    {
        Task<bool> Add(StudentModel model);

        Task<bool> Delete(Student model);

        Task<bool> Update(Student model);

        Task<Student> Get(long Id);

        Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage reqMsg);
    }
}
