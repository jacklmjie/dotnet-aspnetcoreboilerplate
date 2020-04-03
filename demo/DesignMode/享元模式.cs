using System.Collections;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 运用共享技术有效地支持大量细粒度的对象.
    /// 用了享元模式，所以有了共享对象，实例总数就大大减少了
    /// </summary>
    public class 享元模式
    {
        [Fact]
        public void Test()
        {
            //代码外部状态
            int extrinsicstate = 22;
            FlyweightFactory flyweightFactory = new FlyweightFactory();
            Flyweight x = flyweightFactory.GetFlyweight("x");
            x.Operation(--extrinsicstate);

            Flyweight y = flyweightFactory.GetFlyweight("y");
            y.Operation(--extrinsicstate);

            Flyweight uf = new UnsharedConcreteFlyweight();
            uf.Operation(--extrinsicstate);
        }

        /// <summary>
        /// 享元工厂，创建并管理Flyweight对象
        /// </summary>
        public class FlyweightFactory
        {
            private Hashtable flyweights = new Hashtable();
            public FlyweightFactory()
            {
                flyweights.Add("x", new ConcreteFlyweight());
                flyweights.Add("y", new ConcreteFlyweight());
                flyweights.Add("z", new ConcreteFlyweight());
            }
            public Flyweight GetFlyweight(string key)
            {
                return ((Flyweight)flyweights[key]);
            }
        }

        /// <summary>
        /// 享元类的超类或接口，可以接受并作用于外部状态
        /// </summary>
        public abstract class Flyweight
        {
            public abstract void Operation(int extrinsicstate);
        }

        /// <summary>
        /// 具体继承Flyweight超类或实现Flyweight的接口
        /// </summary>
        public class ConcreteFlyweight : Flyweight
        {
            public override void Operation(int extrinsicstate)
            {
                System.Console.WriteLine("具体Flyweight:" + extrinsicstate);
            }
        }

        /// <summary>
        /// 不需要共享的Flyweight子类
        /// </summary>
        public class UnsharedConcreteFlyweight : Flyweight
        {
            public override void Operation(int extrinsicstate)
            {
                System.Console.WriteLine("不共享的具体Flyweight:" + extrinsicstate);
            }
        }
    }
}
