using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 将一个复杂对象的构建与它的表示分离
    /// 使得同样的构建过程可以创建不同的表示
    /// </summary>
    public class 建造者模式
    {
        [Fact]
        public void Test()
        {
            Director director = new Director();
            Builder b1 = new ConcreteBuilder1();
            director.Construct(b1);//指挥者用ConcreteBuilder1方法创建产品
            Product p1 = b1.GetResult();
            p1.Show();

            Builder b2 = new ConcreteBuilder2();
            director.Construct(b2);//指挥者用ConcreteBuilder2方法创建产品
            Product p2 = b2.GetResult();
            p2.Show();
        }
        /// <summary>
        /// 产品类，由多给部件组成
        /// </summary>
        class Product
        {
            List<string> parts = new List<string>();
            public void Add(string part)
            {
                parts.Add(part);
            }
            public void Show()
            {
                System.Console.WriteLine("产品创建");
                foreach (var item in parts)
                {
                    System.Console.WriteLine(item);
                }
            }
        }
        /// <summary>
        /// 指挥者类
        /// </summary>
        class Director
        {
            public void Construct(Builder builder)
            {
                builder.BuildPartA();
                builder.BuildPartB();
            }
        }
        /// <summary>
        /// 抽象建造类
        /// </summary>
        abstract class Builder
        {
            public abstract void BuildPartA();
            public abstract void BuildPartB();
            public abstract Product GetResult();
        }
        /// <summary>
        /// 具体建造类1
        /// </summary>
        class ConcreteBuilder1 : Builder
        {
            Product product = new Product();
            public override void BuildPartA()
            {
                product.Add("部件A");
            }

            public override void BuildPartB()
            {
                product.Add("部件B");
            }

            public override Product GetResult()
            {
                return product;
            }
        }
        /// <summary>
        /// 具体建造类2
        /// </summary>
        class ConcreteBuilder2 : Builder
        {
            Product product = new Product();
            public override void BuildPartA()
            {
                product.Add("部件X");
            }

            public override void BuildPartB()
            {
                product.Add("部件Y");
            }

            public override Product GetResult()
            {
                return product;
            }
        }
    }
}
