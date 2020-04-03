using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 动态地给一个对象添加一些额外的职责
    /// </summary>
    public class 装饰模式
    {
        [Fact]
        public void Test()
        {
            Person xc = new Person("小哈");
            TShirts shirts = new TShirts();
            BigTrouser bigTrouser = new BigTrouser();
            shirts.Decorate(xc);
            bigTrouser.Decorate(xc);
            bigTrouser.Show();
        }

        /// <summary>
        /// 人类
        /// </summary>
        class Person
        {
            public Person()
            {

            }

            private readonly string _name;
            public Person(string name)
            {
                _name = name;
            }

            public virtual void Show()
            {
                Console.WriteLine($"装扮的{_name}");
            }
        }

        /// <summary>
        /// 服饰类
        /// </summary>
        class Finery : Person
        {
            private Person _component;

            /// <summary>
            /// 装扮
            /// </summary>
            /// <param name="component"></param>
            public void Decorate(Person component)
            {
                _component = component;
            }

            public override void Show()
            {
                if (_component != null)
                {
                    _component.Show();
                }
            }
        }

        /// <summary>
        /// 具体服饰类
        /// </summary>
        class TShirts : Finery
        {
            public override void Show()
            {
                Console.WriteLine("大T桖");
                base.Show();
            }
        }

        class BigTrouser : Finery
        {
            public override void Show()
            {
                Console.WriteLine("垮裤");
                base.Show();
            }
        }
    }
}
