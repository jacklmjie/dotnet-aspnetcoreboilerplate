using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 定义一个用于创建对象的接口，让子类决定实例化哪一个子类。工厂方法使一个类的实例化延迟到其子类
    /// </summary>
    public class 工厂方法模式
    {
        [Fact]
        public void Test()
        {
            IFactory factory = new AddFactory();
            Operation oper = factory.CreateOperation();
            oper.numberA = 1;
            oper.numberB = 2;
            double result = oper.GetResult();
        }
        class Operation
        {
            public double numberA { get; set; }
            public double numberB { get; set; }
            public virtual double GetResult()
            {
                double result = 0;
                return result;
            }
        }
        class OperationAdd : Operation
        {
            public override double GetResult()
            {
                double result = numberA + numberB;
                return result;
            }
        }
        interface IFactory
        {
            Operation CreateOperation();
        }
        class AddFactory : IFactory
        {
            public Operation CreateOperation()
            {
                return new OperationAdd();
            }
        }
    }
}
