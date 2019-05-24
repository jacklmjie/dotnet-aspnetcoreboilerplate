using AutoMapper;
using System;

namespace Core.Mapper
{
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        DateTime ITypeConverter<string, DateTime>.Convert(string source, DateTime destination, ResolutionContext context)
        {
            try
            {
                return Convert.ToDateTime(source);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
