using AutoMapper;

namespace Core.Contract
{
    public abstract class MappingContract
    {
        public readonly IMapper _mapper;
        public MappingContract(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
