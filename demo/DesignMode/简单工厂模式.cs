using Xunit;

namespace DesignMode
{
    public class 简单工厂模式
    {
        [Fact]
        public void Test()
        {
            Operation oper;
            oper = OperationFactory.CreateOperation("+");
            oper.numberA = 1;
            oper.numberB = 2;
            double result = oper.GetResult();
        }

        public class OperationFactory
        {
            public static Operation CreateOperation(string operat)
            {
                Operation oper = null;
                switch (operat)
                {
                    case "+":
                        oper = new OperationAdd();
                        break;
                    case "-":
                        oper = new OperationSub();
                        break;
                }
                return oper;
            }
        }

        public class Operation
        {
            public double numberA { get; set; }
            public double numberB { get; set; }
            public virtual double GetResult()
            {
                double result = 0;
                return result;
            }
        }
        private class OperationAdd : Operation
        {
            public override double GetResult()
            {
                double result = numberA + numberB;
                return result;
            }
        }
        private class OperationSub : Operation
        {
            public override double GetResult()
            {
                double result = numberA - numberB;
                return result;
            }
        }
    }
}
