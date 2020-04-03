using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 用一个中介对象来封装一系列的对象交互。
    /// 中介者使各对象不需要显式地相互引用，从而使其耦合松散，而且可以独立地改变它们之间的交互
    /// 控制了集中化，一般应用于一组对象已定义良好但是复杂的方式进行通信的场合
    /// 例：计算器 菜单控件 文本控件 多个按钮 和一个form窗体
    /// </summary>
    public class 中介者模式
    {
        [Fact]
        public void Tets()
        {
            ConcreteMediator mediator = new ConcreteMediator();

            //让两个具体类认识中介对象
            ConcreteColleague1 colleague1 = new ConcreteColleague1(mediator);
            ConcreteColleague2 colleague2 = new ConcreteColleague2(mediator);

            mediator.colleague1 = colleague1;
            mediator.colleague2 = colleague2;

            colleague1.Send("吃过饭了吗？");
            colleague2.Send("没有呢，你打算请客吗？");
            colleague1.Send("呵呵！");
        }
        /// <summary>
        /// 抽象中介者类
        /// </summary>
        abstract class Mediator
        {
            public abstract void Send(string message, Colleague colleague);
        }
        /// <summary>
        /// 具体中介者类
        /// </summary>
        class ConcreteMediator : Mediator
        {
            public ConcreteColleague1 colleague1 { get; set; }
            public ConcreteColleague2 colleague2 { get; set; }

            public override void Send(string message, Colleague colleague)
            {
                if (colleague == colleague1)
                {
                    colleague2.Notify(message);
                }
                else
                {
                    colleague1.Notify(message);
                }
            }
        }
        /// <summary>
        /// 抽象同事类
        /// </summary>
        abstract class Colleague
        {
            protected Mediator mediator;
            public Colleague(Mediator mediator)
            {
                this.mediator = mediator;
            }
        }
        /// <summary>
        /// 具体同事1
        /// </summary>
        class ConcreteColleague1 : Colleague
        {
            public ConcreteColleague1(Mediator mediator) : base(mediator)
            {
            }
            public void Send(string message)
            {
                mediator.Send(message, this);
            }
            public void Notify(string message)
            {
                System.Console.WriteLine($"ConcreteColleague1得到消息{message}");
            }
        }
        /// <summary>
        /// 具体同事2
        /// </summary>
        class ConcreteColleague2 : Colleague
        {
            public ConcreteColleague2(Mediator mediator) : base(mediator)
            {
            }
            public void Send(string message)
            {
                mediator.Send(message, this);
            }
            public void Notify(string message)
            {
                System.Console.WriteLine($"ConcreteColleague2得到消息{message}");
            }
        }
    }
}
