using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 将一个类的接口转换成客户希望的另外一个接口
    /// Adapter模式使得原本由于接口不兼容而不能一起工作的那些类可以一起工作
    /// </summary>
    public class 适配器模式
    {
        [Fact]
        public void Test()
        {
            Target target = new Adappter();
            //其实请求的是适配后的接口了
            target.Request();
        }
        /// <summary>
        /// 通过在内部包装一个Adaptee对象，把源接口转换成目标接口
        /// </summary>
        class Adappter:Target
        {
            private Adaptee adaptee = new Adaptee();
            public override void Request()
            {
                adaptee.SpecificRequest();
            }
        }
        /// <summary>
        /// 需要适配的类
        /// </summary>
        class Adaptee
        {
            public void SpecificRequest()
            {
                Console.WriteLine("特殊请求");
            }
        }

        /// <summary>
        /// 这是客户所期待的接口。目标可以是具体的或抽象的类，也可以是接口
        /// </summary>
        class Target
        {
            public virtual void Request()
            {
                Console.WriteLine("普通请求");
            }
        }
    }
}
