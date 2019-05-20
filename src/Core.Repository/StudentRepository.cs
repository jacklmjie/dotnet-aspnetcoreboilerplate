using Core.Entity;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext dbContext)
          : base(dbContext)
        { }

        public override int Add(Student entity, bool IsCommit = false)
        {
            return base.Add(entity, IsCommit);
        }
    }
}
