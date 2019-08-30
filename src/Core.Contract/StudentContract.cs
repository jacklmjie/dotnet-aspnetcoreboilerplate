using AutoMapper;
using Core.Common;
using Core.IContract;
using Core.IRepository;
using Core.Models;
using Core.Models.Identity.Entity;
using System.Threading.Tasks;

namespace Core.Contract
{
    public class StudentService : MappingContract, IStudentContract
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IMapper mapper,
        IStudentRepository studentRepository) : base(mapper)
        {
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
