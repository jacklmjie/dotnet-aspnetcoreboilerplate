using Core.Entity;
using Core.IRepository;
using Core.IService;

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

        public int Add(Student student)
        {
            //TODO:把StudentViewModel映射到Student
            var model = new Student() { Name = "test", Age = 1, Sex = 0, ClassRoom = 1 };
            var task =  _studentRepository.Add(model);
            task.Wait();
            return task.Result;
        }

        public bool AddUnit(Student student)
        {
            var model = new Student();
            _studentRepository.Add(model);
            _studentRepository.Add(model);
            return true;
        }
    }
}
