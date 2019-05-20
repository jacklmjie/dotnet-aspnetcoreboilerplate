using Core.Entity;

namespace Core.IRepository
{
    public interface IStudentRepository
    {
        int Add(Student entity, bool IsCommit = false);
    }
}
