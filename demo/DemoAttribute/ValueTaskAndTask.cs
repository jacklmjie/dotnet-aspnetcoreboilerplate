using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoAttribute
{
    /// <summary>
    /// Task是一个类，类创建是在托管堆上,赋和传递的是引用地址，等待没有引用之后就被GC回收掉
    /// ValueTask是值类型，放在栈中,赋和传递的是值，即时被释放，避免不必要的内存开销
    /// </summary>
    public class ValueTaskAndTask
    {
        [Fact]
        public void Test()
        {
            //brefore
            //Task.FromResult(42);

            //after
            //new ValueTask(42);

            //同步情况下，不再用了，ValueTask可以避免不必要的内存开销
        }
    }
}
