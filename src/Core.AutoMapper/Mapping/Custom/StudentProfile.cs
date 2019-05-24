using AutoMapper;
using Core.Entity;
using Core.Models;

namespace Core.Mapper
{
    ///// <summary>
    ///// 自定义映射
    ///// </summary>
    //public class StudentProfile : Profile
    //{
    //    public StudentProfile()
    //    {
    //        CreateMap<StudentModel, Student>()
    //            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    //            .ForMember(dest => dest.Age, opt => opt.Ignore())
    //            .ForMember(dest => dest.Age, opt => opt.MapFrom(new StudentResolver()))
    //            .ConvertUsing(typeof(StudentConverter)); ;
    //        //验证类型映射是否正确
    //        //Mapper.AssertConfigurationIsValid();
    //    }
    //}

    ///// <summary>
    ///// 自定义解析器
    ///// </summary>
    //public class StudentResolver : IValueResolver<StudentModel, Student, int>
    //{
    //    public int Resolve(StudentModel source, Student destination, int destMember, ResolutionContext context)
    //    {
    //        return source.Age + source.Age;
    //    }
    //}

    ///// <summary>
    ///// 自定义转换器
    ///// </summary>
    //public class StudentConverter : ITypeConverter<StudentModel, Student>
    //{
    //    public Student Convert(StudentModel source, Student destination, ResolutionContext context)
    //    {
    //        destination.Age = System.Convert.ToInt32(source.Age);
    //        return destination;
    //    }
    //}
}
