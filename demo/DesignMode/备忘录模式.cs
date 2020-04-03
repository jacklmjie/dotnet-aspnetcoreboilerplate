using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 在不破坏封装的情况下，捕获一个对象的内部状态，并在该对象之外保存这个状态
    /// 这样以后就可将该对象恢复到原先保存的状态
    /// </summary>
    public class 备忘录模式
    {
        [Fact]
        public void Test()
        {
            Originator originator = new Originator();
            originator.State = "ON";
            originator.Show();

            Caretaker caretaker = new Caretaker();
            caretaker.Memento = originator.CreateMemento();

            originator.State = "OFF";
            originator.Show();

            //回复初始状态
            originator.SetMemento(caretaker.Memento);
            originator.Show();
        }
        /// <summary>
        /// 发起人类
        /// </summary>
        class Originator
        {
            /// <summary>
            /// 需要保存的熟悉，可能有多个
            /// </summary>
            public string State { get; set; }
            public Memento CreateMemento()
            {
                return new Memento(State);
            }
            public void SetMemento(Memento memento)
            {
                State = memento.State;
            }
            public void Show()
            {
                System.Console.WriteLine($"State={State}");
            }
        }

        /// <summary>
        /// 备忘录类
        /// </summary>
        class Memento
        {
            public Memento(string state)
            {
                State = state;
            }
            public string State { get; }
        }

        /// <summary>
        /// 管理类
        /// </summary>
        class Caretaker
        {
            public Memento Memento { get; set; }
        }
    }
}
