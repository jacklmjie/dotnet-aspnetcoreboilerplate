using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 为子系统的一组接口提供一个一致的页面
    /// 此模式定义了一个高层接口，这个接口使得这一子系统更加容易使用
    /// </summary>
    public class 外观模式
    {
        [Fact]
        public void Test()
        {
            Facade facade = new Facade();
            facade.MethodA();
        }

        class Facade
        {
            SubSystemOne one;
            SubSystemTwo two;
            public Facade()
            {
                one = new SubSystemOne();
                two = new SubSystemTwo();
            }
            public void MethodA()
            {
                one.MethodOne();
                two.MethodTwo();
                System.Console.WriteLine("方法组A");
            }
            public void MethodB()
            {
                two.MethodTwo();
                one.MethodOne();
                System.Console.WriteLine("方法组B");
            }
        }

        class SubSystemOne
        {
            public void MethodOne()
            {
                System.Console.WriteLine("子系统方法一");
            }
        }
        class SubSystemTwo
        {
            public void MethodTwo()
            {
                System.Console.WriteLine("子系统方法二");
            }
        }
    }
}
