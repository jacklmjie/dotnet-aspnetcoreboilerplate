using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 用原型实例指定创建对象的种类，并且通过拷贝这些原型创建新的对象
    /// </summary>
    public class 原型模式
    {
        [Fact]
        public void Test()
        {
            ProtoType a = new ProtoType
            {
                Name = "a"
            };
            a.SetWork("工作a");
            //浅复制，引用的对象数据不会被复制
            ProtoType b = (ProtoType)a.Clone();
            b.Name = "b";
            b.SetWork("工作b");
            //深复制考虑循环引用问题
            //DataSet的Clone()复制结构和Copy()复制结构和数据
            a.Display();
            b.Display();
        }

        class ProtoType : ICloneable
        {
            public string Name { get; set; }
            WorkExperience work;
            public ProtoType()
            {
                work = new WorkExperience();
            }
            public void SetWork(string job)
            {
                work.Job = job;
            }
            public void Display()
            {
                Console.WriteLine($"{Name}");
                Console.WriteLine($"工作:{work.Job}");
            }

            public ProtoType(WorkExperience work)
            {
                //深复制,
                this.work = (WorkExperience)work.Clone();
            }
            public object Clone()
            {
                ProtoType protoType = new ProtoType(this.work)
                {
                    Name = this.Name
                };
                return protoType;
            }
        }
        class WorkExperience : ICloneable
        {
            public string Job { get; set; }

            public object Clone()
            {
                //浅复制，引用的对象数据不会被复制
                return MemberwiseClone();
            }
        }
    }
}
