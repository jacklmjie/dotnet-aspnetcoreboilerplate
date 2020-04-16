using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 给定一个语言，定义它的文法的一种表示，并定义一个解释器，
    /// 这个解释器使用该表示来解释语言中的句子.
    /// 将一句话，转变成实际的命令程序执行.
    /// </summary>
    public class 解释器模式
    {
        [Fact]
        public void Test()
        {
            Context context = new Context();
            IList<AbstractExpression> list = new List<AbstractExpression>();
            list.Add(new TerminamExpression());
            list.Add(new NoterminamExpression());
            list.Add(new TerminamExpression());
            foreach (var exp in list)
            {
                exp.Interpret(context);
            }
        }

        /// <summary>
        /// 抽象表达式
        /// </summary>
        public abstract class AbstractExpression
        {
            public abstract void Interpret(Context context);
        }

        /// <summary>
        /// 终端解释器
        /// </summary>
        public class TerminamExpression : AbstractExpression
        {
            public override void Interpret(Context context)
            {
                System.Console.WriteLine("终端解释器");
            }
        }

        /// <summary>
        /// 非终端解释器
        /// </summary>
        public class NoterminamExpression : AbstractExpression
        {
            public override void Interpret(Context context)
            {
                System.Console.WriteLine("非终端解释器");
            }
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public class Context
        {
            public string Input { get; set; }
            public string Output { get; set; }
        }
    }
}
