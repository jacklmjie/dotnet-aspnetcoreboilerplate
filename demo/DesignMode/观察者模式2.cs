using System;
using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 事件委托实现观察者模式
    /// </summary>
    public class 观察者模式2
    {
        [Fact]
        public void Test()
        {
            Boss boss = new Boss();
            StockObserver stockObserver = new StockObserver("A", boss);
            NBAObserver nBAObserver = new NBAObserver("B", boss);

            boss.Update += new EventHandler(stockObserver.CloseStockMarket);
            boss.Update += new EventHandler(nBAObserver.CloseNBADirectSeeding);

            boss.SubjectState = "大BOSS回来了";
            //发出通知
            boss.Notify();
        }
        /// <summary>
        /// 通知者接口
        /// </summary>
        interface Subject
        {
            void Notify();
            string SubjectState { get; set; }
        }
        delegate void EventHandler();
        class Boss : Subject
        {
            public event EventHandler Update;
            public void Notify()
            {
                Update();
            }
            public string SubjectState { get; set; }
        }

        /// <summary>
        /// 看股票的观察者
        /// </summary>
        class StockObserver
        {
            private string _name;
            private Subject _subject;
            public StockObserver(string name,Subject subject)
            {
                _name = name;
                _subject = subject;
            }
            public void CloseStockMarket()
            {
                System.Console.WriteLine($"{_subject.SubjectState} {_name} 关闭股票，继续工作！");
            }
        }
        /// <summary>
        /// 看NBA同事
        /// </summary>
        class NBAObserver
        {
            private string _name;
            private Subject _subject;
            public NBAObserver(string name, Subject subject)
            {
                _name = name;
                _subject = subject;
            }
            /// <summary>
            /// 关闭NBA直播
            /// </summary>
            public void CloseNBADirectSeeding()
            {
                System.Console.WriteLine($"{_subject.SubjectState} {_name} 关闭NBA直播，继续工作！");
            }
        }
    }
}
