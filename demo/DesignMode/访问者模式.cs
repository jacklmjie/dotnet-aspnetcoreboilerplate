using System;
using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 表示一个作用于某对象结构中的各元素的操作.
    /// 它可以使你在不改变元素的类的前提下定义作用于这些元素的新操作.
    /// </summary>
    public class 访问者模式
    {
        [Fact]
        public void Test()
        {
            ObjectStrucuter strucuter = new ObjectStrucuter();
            strucuter.Add(new Man());
            strucuter.Add(new Woman());

            //成功时的反应
            Success v1 = new Success();
            strucuter.Display(v1);

            ////失败的反应
            //Failing v2 = new Failing();
            //strucuter.Display(v2);
        }

        /// <summary>
        /// 对象结构类，需要男人和女人不同状态的对比，针对不同的状态遍历得到不同的反应
        /// </summary>
        public class ObjectStrucuter
        {
            private List<Person> elements = new List<Person>();

            /// <summary>
            /// 增加
            /// </summary>
            /// <param name="element"></param>
            public void Add(Person element)
            {
                elements.Add(element);
            }

            /// <summary>
            /// 移除
            /// </summary>
            /// <param name="element"></param>
            public void Remove(Person element)
            {
                elements.Remove(element);
            }

            /// <summary>
            /// 查看显示
            /// </summary>
            /// <param name="element"></param>
            public void Display(Visitor visitor)
            {
                foreach (var e in elements)
                {
                    e.Accept(visitor);
                }
            }
        }

        /// <summary>
        /// 状态的抽象类,重点在状态里男人和女人是固定的，否自访问者模式不适用
        /// </summary>
        public abstract class Visitor
        {
            /// <summary>
            /// 得到男人的反应
            /// </summary>
            public abstract void GetManConclusion(Man person);

            /// <summary>
            /// 得到女人的反应
            /// </summary>
            public abstract void GetWomanConclusion(Woman person);
        }

        /// <summary>
        /// 成功状态
        /// </summary>
        public class Success : Visitor
        {
            public override void GetManConclusion(Man person)
            {
                Console.WriteLine("成功时男人的反应");
            }

            public override void GetWomanConclusion(Woman person)
            {
                Console.WriteLine("成功时女人的反应");
            }
        }

        ///// <summary>
        ///// 失败状态：同成功状态
        ///// </summary>
        //public class Failing : Visitor { }


        /// <summary>
        /// 人的抽象类
        /// </summary>
        public abstract class Person
        {
            //接受
            public abstract void Accept(Visitor visitor);
        }

        /// <summary>
        /// 男人
        /// </summary>
        public class Man : Person
        {
            public override void Accept(Visitor visitor)
            {
                visitor.GetManConclusion(this);
            }
        }

        /// <summary>
        /// 女人
        /// </summary>
        public class Woman : Person
        {
            public override void Accept(Visitor visitor)
            {
                visitor.GetWomanConclusion(this);
            }
        }
    }
}
