using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DemoAttribute
{
    public class ChangeToken观察者
    {
        [Fact]
        public void Test()
        {
            Console.WriteLine("开始监测文件夹 watch");

            //dotnet命令所在的目录即工作目录
            //var path = Environment.CurrentDirectory
            //var path = Directory.GetCurrentDirectory()
            var path = Path.GetDirectoryName(typeof(ChangeToken观察者).Assembly.Location);
            var phyFileProvider = new PhysicalFileProvider(path + "../../../../watch");
            IChangeToken changeToken = phyFileProvider.Watch("*.*");
            changeToken.RegisterChangeCallback(_ =>
            {
                Console.WriteLine("检测到文件夹有变化1!" + _);
            }, "xiaoming");

            ChangeToken.OnChange(
               () => phyFileProvider.Watch("*.*"),
               () => Console.WriteLine("检测到文件夹有变化2!")
            );

            Console.ReadLine();
        }
    }
}
