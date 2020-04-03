using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 定义一个操作中的算法的骨架，而将一些步骤延迟到子类中
    /// 模板方法使得子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤
    /// </summary>
    public class 模板方法
    {
        [Fact]
        public void Test()
        {
            AbstractClass c;
            c = new ConcreteClassA();
            c.TemplateMethod();

            c = new ConcreteClassB();
            c.TemplateMethod();
        }
        abstract class AbstractClass
        {
            public abstract void PrimitiveOperation1();
            public abstract void PrimitiveOperation2();
            public void TemplateMethod()
            {
                PrimitiveOperation1();
                PrimitiveOperation2();
                Console.WriteLine();
            }
        }
        class ConcreteClassA : AbstractClass
        {
            public override void PrimitiveOperation1()
            {
                throw new NotImplementedException();
            }

            public override void PrimitiveOperation2()
            {
                throw new NotImplementedException();
            }
        }
        class ConcreteClassB : AbstractClass
        {
            public override void PrimitiveOperation1()
            {
                throw new NotImplementedException();
            }

            public override void PrimitiveOperation2()
            {
                throw new NotImplementedException();
            }
        }
    }
}
