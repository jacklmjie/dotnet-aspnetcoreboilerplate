using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 将一个请求封装为一个对象，从而使你可用不同的请求对客户进行参数化；
    /// 对请求排队或记录请求日志，以及支持可撤销的操作.
    /// </summary>
    public class 命令模式
    {
        [Fact]
        public void Test()
        {
            Receiver receiver = new Receiver();
            Command command = new ConcreteCommand(receiver);
            Invocker invocker = new Invocker();

            invocker.SetCommand(command);
            invocker.ExecuteCommand();
        }

        /// <summary>
        /// 执行操作的接口
        /// </summary>
        abstract class Command
        {
            protected Receiver _receiver;
            public Command(Receiver receiver)
            {
                _receiver = receiver;
            }

            public abstract void Execute();
        }
        /// <summary>
        /// 将一个接收者对象绑定于一个动作，调用接收者相应的操作，以实现Execute
        /// 可以有多个命令,实现命令Command
        /// </summary>
        class ConcreteCommand : Command
        {
            public ConcreteCommand(Receiver receiver) : base(receiver)
            {

            }
            public override void Execute()
            {
                _receiver.Action();
            }
        }
        /// <summary>
        /// 要求该命令执行这个请求
        /// </summary>
        class Invocker
        {
            private Command _command;
            public void SetCommand(Command command)
            {
                _command = command;
            }
            public void ExecuteCommand()
            {
                _command.Execute();
            }
        }
        /// <summary>
        /// 接受者类
        /// </summary>
        class Receiver
        {
            public void Action()
            {
                System.Console.WriteLine("执行请求");
            }
        }
    }
}
