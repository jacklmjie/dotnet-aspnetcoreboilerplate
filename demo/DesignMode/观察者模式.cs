using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 定义了一种一对多的依赖关系，让多个观察对象同时监听某一主题内容
    /// 这个主题内容再状态发生变化时，会通知所有观察者对象，使他们能够自动更新自己
    /// </summary>
    public class 观察者模式
    {
        [Fact]
        public void Test()
        {
            ConcreteSubject subject = new ConcreteSubject();

            subject.Attach(new ConcreteObserver(subject, "X"));
            subject.Attach(new ConcreteObserver(subject, "Y"));
            subject.Attach(new ConcreteObserver(subject, "Z"));

            subject.SubjectState = "ABC";
            subject.Notify();
        }
        /// <summary>
        /// 抽象通知者
        /// </summary>
        abstract class Subject
        {
            private List<Observer> observers = new List<Observer>();
            /// <summary>
            /// 增加观察者
            /// </summary>
            /// <param name="observer"></param>
            public void Attach(Observer observer)
            {
                observers.Add(observer);
            }
            /// <summary>
            /// 移除观察者
            /// </summary>
            /// <param name="observer"></param>
            public void Detach(Observer observer)
            {
                observers.Remove(observer);
            }
            /// <summary>
            /// 通知
            /// </summary>
            public void Notify()
            {
                foreach (var item in observers)
                {
                    item.Update();
                }
            }
        }
        /// <summary>
        /// 具体通知者
        /// </summary>
        class ConcreteSubject : Subject
        {
            public string SubjectState { get; set; }
        }
        /// <summary>
        /// 抽象观察者
        /// </summary>
        abstract class Observer
        {
            public abstract void Update();
        }
        class ConcreteObserver : Observer
        {
            private string _name;
            private string _observerState;
            public ConcreteObserver(ConcreteSubject subject, string name)
            {
                this._name = name;
                this.Subject = subject;
            }
            public override void Update()
            {
                _observerState = Subject.SubjectState;
                System.Console.WriteLine($"观察者{_name}的新状态是{_observerState}");
            }
            public ConcreteSubject Subject { get; set; }
        }
    }
}
