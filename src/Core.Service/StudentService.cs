using Core.Common;
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

        public async Task<bool> Add(Student entity)
        {
            return await _studentRepository.Add(entity);
        }

        public async Task<bool> Delete(Student entity)
        {
            return await _studentRepository.Delete(entity);
        }

        public async Task<bool> Update(Student entity)
        {
            return await _studentRepository.Update(entity);
        }

        public async Task<Student> Get(long Id)
        {
            return await _studentRepository.Get(Id);
        }

        public async Task<QueryResponseByPage<Student>> GetListPaged(QueryRequestByPage reqMsg)
        {
            var count = await _studentRepository.GetCount(reqMsg);
            var list = await _studentRepository.GetListPaged(reqMsg);
            return QueryResponseByPage<Student>.Create(count,list, reqMsg);
        }
    }
}
