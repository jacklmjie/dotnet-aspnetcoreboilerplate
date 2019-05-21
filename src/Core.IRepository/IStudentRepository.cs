using Core.Entity;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IStudentRepository
    {
        Task<int> Add(Student entity);
    }
}
