using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 为其他对象提供一种代理以控制对这个对象的访问
    /// </summary>
    public class 代理模式
    {
        [Fact(DisplayName = "代理")]
        public void Test()
        {
            Proxy proxy = new Proxy();
            proxy.Request();
        }
        [Fact(DisplayName = "代理运用")]
        public void Test2()
        {
            Girl mm = new Girl();
            mm.Name = "beautiful";
            PursuitProxy proxy = new PursuitProxy(mm);
            proxy.GiveFlowers();
        }

        abstract class Subject
        {
            public abstract void Request();
        }
        class RealSubject : Subject
        {
            public override void Request()
            {
                Console.WriteLine("真实的请求");
            }
        }
        class Proxy : Subject
        {
            RealSubject realSubject;
            public override void Request()
            {
                if (realSubject == null)
                {
                    realSubject = new RealSubject();
                }
                realSubject.Request();
            }
        }

        interface IGiveGift
        {
            void GiveFlowers();
        }
        class Pursuit : IGiveGift
        {
            public void GiveFlowers()
            {
                throw new NotImplementedException();
            }
        }
        class Girl
        {
            public string Name { get; set; }
        }
        class PursuitProxy : IGiveGift
        {
            Pursuit gg;
            public PursuitProxy(Girl mm)
            {
                gg = new Pursuit();
            }
            public void GiveFlowers()
            {
                gg.GiveFlowers();
            }
        }
    }
}
