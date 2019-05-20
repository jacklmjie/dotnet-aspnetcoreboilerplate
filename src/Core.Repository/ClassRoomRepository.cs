using Core.Entity;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class ClassRoomRepository : Repository<Student>, IClassRoomRepository
    {
        public ClassRoomRepository(DbContext dbContext)
           : base(dbContext)
        { }

    }
}
