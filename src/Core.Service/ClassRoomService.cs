using Core.IRepository;
using Core.IService;

namespace Core.Service
{
    public class ClassRoomService: IClassRoomService
    {
        private readonly IClassRoomRepository _classRoomRepository;
        public ClassRoomService(IClassRoomRepository classRoomRepository)
        {
            _classRoomRepository = classRoomRepository;
        }
    }
}
