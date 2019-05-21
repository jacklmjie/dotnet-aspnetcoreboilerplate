using Core.Entity;

namespace Core.IService
{
    public interface IStudentService
    {
        int Add(Student student);

        bool AddUnit(Student student);
    }
}
