using AutoMapper;
using Core.Common;
using Core.Entity;
using Core.IRepository;
using Core.IService;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Service
{
    public class StudentService : MappingService, IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRoomRepository _classRoomRepository;

        public StudentService(IMapper mapper,
        IStudentRepository studentRepository,
        IClassRoomRepository classRoomRepository) : base(mapper)
        {
            _classRoomRepository = classRoomRepository;
            _studentRepository = studentRepository;
        }

        public async Task<int?> Add(StudentModel model)
        {
            var entity = _mapper.Map<StudentModel, Student>(model);
            return await _studentRepository.Add(entity);
        }

        public async Task<bool> Delete(Student model)
        {
            return await _studentRepository.Delete(model);
        }

        public async Task<bool> Update(Student model)
        {
            return await _studentRepository.Update(model);
        }

        public async Task<Student> Get(long Id)
        {
            return await _studentRepository.Get(Id);
        }

        public async Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage reqMsg)
        {
            var count = await _studentRepository.GetCount(reqMsg);
            var list = await _studentRepository.GetListPaged(reqMsg);
            return QueryResponseByPage<Student>.Create(count, list, reqMsg);
        }
    }
}
