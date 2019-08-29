using AutoMapper;
using Core.Common;
using Core.IRepository;
using Core.IService;
using Core.Models;
using Core.Models.Identity.Entity;
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

        public async Task<long> Add(StudentDto dto)
        {
            var entity = _mapper.Map<StudentDto, Student>(dto);
            return await _studentRepository.Add(entity);
        }

        public async Task<bool> Delete(Student dto)
        {
            return await _studentRepository.Delete(dto);
        }

        public async Task<bool> Update(Student dto)
        {
            return await _studentRepository.Update(dto);
        }

        public async Task<Student> Get(long Id)
        {
            return await _studentRepository.Get(Id);
        }

        public async Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage dto)
        {
            var count = await _studentRepository.GetCount(dto);
            var list = await _studentRepository.GetListPaged(dto);
            return QueryResponseByPage<Student>.Create(count, list, dto);
        }
    }
}
