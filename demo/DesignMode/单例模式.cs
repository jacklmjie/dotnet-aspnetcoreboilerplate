using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 保证一个类仅有一个实例，并提供一个访问它的全局访问点
    /// </summary>
    public class 单例模式
    {
        [Fact]
        public void Test()
        {
            Singleton s1 = Singleton.GetInstance();
            Singleton2 s2 = Singleton2.GetInstance();
        }

        /// <summary>
        /// 懒汉式单例类
        /// </summary>
        class Singleton
        {
            private static Singleton instance;
            private static readonly object syncRoot = new object();
            public static Singleton GetInstance()
            {
                //双重锁定
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Singleton();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 饿汉式单例类
        /// </summary>
        public sealed class Singleton2
        {
            //sealed组织发生派生，而派生可能会增加实例
            /// <summary>
            /// 在第一次引用类的任何成员时创建实例。公共语言运行库负责处理变量初始化
            /// 要提前占用系统资源
            /// </summary>
            private static readonly Singleton2 instance = new Singleton2();
            private Singleton2() { }
            public static Singleton2 GetInstance()
            {
                return instance;
            }
        }
    }
}
