using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 使多个对象都有机会处理请求，从而避免请求的发送者和接收者之间的耦合关系。
    /// 将这个对象连成一条链，并沿着这条链传递请求，直到有一个对象处理它为止。
    /// </summary>
    public class 责任链模式
    {
        [Fact]
        public void Test()
        {
            Handler handler1 = new ConcreteHandler1("下级");
            Handler handler2 = new ConcreteHandler1("上级");
            //设置责任链到上一级
            handler1.SetSuccessor(handler2);

            int[] request = { 2, 3, 10, 20 };
            foreach (var item in request)
            {
                handler1.HandleRequest(item);
            }
        }

        /// <summary>
        /// 处理请求的接口
        /// </summary>
        abstract class Handler
        {
            protected string name;
            protected Handler successor;
            public Handler(string name)
            {
                this.name = name;
            }
            /// <summary>
            /// 设置继承者
            /// </summary>
            /// <param name="successor"></param>
            public void SetSuccessor(Handler successor)
            {
                this.successor = successor;
            }
            /// <summary>
            /// 处理请求的抽象方法
            /// </summary>
            /// <param name="request"></param>
            public abstract void HandleRequest(int request);
        }

        /// <summary>
        /// 具体处理类1
        /// </summary>
        class ConcreteHandler1 : Handler
        {
            public ConcreteHandler1(string name)
                : base(name)
            { }
            public override void HandleRequest(int request)
            {
                if (request > 0 && request < 10)
                {
                    Console.WriteLine($"{name}处理请求{request}");
                }
                else
                {
                    //转移到下一位
                    if (successor != null)
                        successor.HandleRequest(request);
                }
            }
        }

        /// <summary>
        /// 具体处理类2
        /// </summary>
        class ConcreteHandler2 : Handler
        {
            public ConcreteHandler2(string name)
                : base(name)
            { }
            public override void HandleRequest(int request)
            {
                Console.WriteLine($"{name}处理请求{request}");
            }
        }
    }
}
