using Core.Common;
using Core.Models;
using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.IContract
{
    public interface IStudentContract
    {
        Task<long> Add(StudentDto dto);

        Task<bool> Delete(Student dto);

        Task<bool> Update(Student dto);

        Task<Student> Get(long Id);

        Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage dto);
    }
}
