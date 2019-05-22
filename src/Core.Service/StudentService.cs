using Core.Entity;
using Core.IRepository;
using Core.IService;
using System.Threading.Tasks;

namespace Core.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRoomRepository _classRoomRepository;
        public StudentService(IStudentRepository studentRepository,
        IClassRoomRepository classRoomRepository)
        {
            _classRoomRepository = classRoomRepository;
            _studentRepository = studentRepository;
        }

        public async Task<int> Add(Student entity)
        {
            return await _studentRepository.Add(entity);
        }
    }
}
