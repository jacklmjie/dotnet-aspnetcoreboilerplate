using Core.Entity;
using System.Threading.Tasks;

namespace Core.IService
{
    public interface IStudentService
    {
        Task<int> Add(Student entity);
    }
}
