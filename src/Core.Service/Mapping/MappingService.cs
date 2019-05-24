using AutoMapper;

namespace Core.Service
{
    public abstract class MappingService
    {
        public readonly IMapper _mapper;
        public MappingService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
