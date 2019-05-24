using AutoMapper;
using Core.Entity;
using Core.Models;

namespace Core.Mapper
{
    /// <summary>
    /// Mapping
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentModel, Student>();      
        }
    }
}
