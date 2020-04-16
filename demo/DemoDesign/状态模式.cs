using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 当一个对象的内在状态改变时允许改变其行为
    /// 这个对象看起来像是改变了类
    /// </summary>
    public class 状态模式
    {
        [Fact]
        public void Test()
        {
            //开启一个项目
            Work work = new Work();

            work.Hour = 9;
            work.WriteProgram();

            work.Hour = 10;
            work.WriteProgram();

            work.Hour = 13;
            work.WriteProgram();


            work.Hour = 19;
            work.TaskFinished = false;
            work.WriteProgram();

            work.Hour = 21;
            work.TaskFinished = true;
            work.WriteProgram();
        }
        /// <summary>
        /// 抽象状态
        /// </summary>
        abstract class State
        {
            public abstract void WriteProgram(Work work);
        }
        /// <summary>
        /// 上午工作状态
        /// </summary>
        class ForenoonState : State
        {
            public override void WriteProgram(Work work)
            {
                if(work.Hour<12)
                {
                    Console.WriteLine($"当前时间:{work.Hour} 上午工作状态");
                }
                else
                {
                    //超过12点，则转入中午工作状态
                    work.SetState(new AfetrenoonState());
                    work.WriteProgram();
                }
            }
        }
        /// <summary>
        /// 下午工作状态
        /// </summary>
        class AfetrenoonState : State
        {
            public override void WriteProgram(Work work)
            {
                if (work.Hour < 17)
                {
                    Console.WriteLine($"当前时间:{work.Hour} 下午工作状态");
                }
                else
                {
                    //超过17点，则转入晚上工作状态
                    work.SetState(new EveningState());
                    work.WriteProgram();
                }
            }
        }
        /// <summary>
        /// 晚上工作状态
        /// </summary>
        class EveningState : State
        {
            public override void WriteProgram(Work work)
            {
                if(work.TaskFinished)
                {
                    Console.WriteLine($"当前时间:{work.Hour} 工作完成 下班状态");
                }
                else
                {
                    Console.WriteLine("额，继续完成工作...");
                }
            }
        }
        /// <summary>
        /// 工作类
        /// </summary>
        class Work
        {
            public double Hour { get; set; }
            public bool TaskFinished { get; set; }
            private State _currentState;
            public Work()
            {
                _currentState = new ForenoonState();
            }
            public void SetState(State state)
            {
                _currentState = state;
            }
            public void WriteProgram()
            {
                _currentState.WriteProgram(this);
            }
        }
    }
}
