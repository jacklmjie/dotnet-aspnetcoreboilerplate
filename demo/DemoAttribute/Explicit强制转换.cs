using System;
using Xunit;

namespace DemoAttribute
{
    public class Explicit强制转换
    {
        [Fact]
        public void Test1()
        {
            var timeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));
            double hours = timeRange;
            Console.WriteLine($"两个时间相差{hours}小时");
        }

        public class DateTimeRange
        {
            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public DateTimeRange(DateTime startTime, DateTime endTime)
            {
                StartTime = startTime;
                EndTime = endTime;
            }

            //operator 后面跟需要转换的类型
            //Explicit
            public static implicit operator double(DateTimeRange timeRange)
            {
                return (timeRange.EndTime - timeRange.StartTime).TotalHours;
            }
        }
    }
}
