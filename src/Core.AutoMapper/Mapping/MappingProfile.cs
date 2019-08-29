using AutoMapper;
using Core.Models;
using Core.Models.Identity.Entity;

namespace Core.Mapper
{
    /// <summary>
    /// Mapping
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentDto, Student>();      
        }
    }
}
