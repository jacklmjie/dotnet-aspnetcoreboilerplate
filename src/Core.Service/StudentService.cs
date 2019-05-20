using Core.Entity;
using Core.IRepository;
using Core.IService;

namespace Core.Service
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRoomRepository _classRoomRepository;
        public StudentService(IUnitOfWork unitOfWork,
        IStudentRepository studentRepository,
        IClassRoomRepository classRoomRepository)
        {
            _unitOfWork = unitOfWork;
            _classRoomRepository = classRoomRepository;
            _studentRepository = studentRepository;
        }

        public void Add(Student student)
        {
            //TODO:需要测试，是否实现了事物，同时添加成功或者同时添加失败
            _studentRepository.Add(student);
            _studentRepository.Add(student);
            _unitOfWork.SaveChanges();
        }
    }
}
