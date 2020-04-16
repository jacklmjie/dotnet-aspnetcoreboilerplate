using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 提供一种方法顺序访问一个聚合对象中各个元素
    /// 而又不暴露该对象的内部表示
    /// </summary>
    public class 迭代器模式
    {
        [Fact]
        public void Test()
        {
            //Net的迭代器
            IList<string> list = new List<string>();
            list.Add("小李");
            list.Add("小红");
            list.Add("小黑");
            foreach (var b in list)
            {
                System.Console.WriteLine($"this is {b}");
            }
            //foreach in 做的工作其实是
            IEnumerator<string> e = list.GetEnumerator();
            while (e.MoveNext())
            {
                System.Console.WriteLine($"this is {e.Current}");
            }

            //GoF的迭代器
            ConcreteAggregate a = new ConcreteAggregate();
            a[0] = "小李";
            a[1] = "小红";
            a[2] = "小黑";

            Iterator i = new ConcreteIterator(a);
            object item = i.First();
            System.Console.WriteLine($"this item is {item}");
            while (!i.IsDone())
            {
                System.Console.WriteLine($"this is {i.CurrentItem()}");
                i.Next();
            }
            System.Console.ReadLine();
        }
        /// <summary>
        /// 迭代器抽象类
        /// </summary>
        abstract class Iterator
        {
            public abstract object First();
            public abstract object Next();
            public abstract bool IsDone();
            public abstract object CurrentItem();
        }

        /// <summary>
        /// 具体迭代器类
        /// </summary>
        class ConcreteIterator : Iterator
        {
            private ConcreteAggregate _aggregate;
            private int current = 0;
            public ConcreteIterator(ConcreteAggregate aggregate)
            {
                _aggregate = aggregate;
            }
            public override object First()
            {
                return _aggregate[0];
            }

            public override object Next()
            {
                object ret = null;
                current++;
                if (current < _aggregate.Count)
                {
                    ret = _aggregate[current];
                }
                return ret;
            }

            public override bool IsDone()
            {
                return current > _aggregate.Count ? true : false;
            }

            public override object CurrentItem()
            {
                return _aggregate[current];
            }
        }

        /// <summary>
        /// 聚集抽象类
        /// </summary>
        abstract class Aggregate
        {
            public abstract Iterator CreateIterator();
        }

        /// <summary>
        /// 具体聚集类
        /// </summary>
        class ConcreteAggregate : Aggregate
        {
            private List<object> items = new List<object>();
            public override Iterator CreateIterator()
            {
                return new ConcreteIterator(this);
            }

            public int Count
            {
                get
                {
                    return items.Count;
                }
            }

            public object this[int index]
            {
                get
                {
                    return items[index];
                }
                set
                {
                    items.Insert(index, value);
                }
            }
        }
    }
}
