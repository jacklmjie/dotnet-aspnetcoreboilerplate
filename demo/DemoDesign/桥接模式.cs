using System;
using Xunit;

namespace DesignMode
{
    /// <summary>
    ///将抽象部分与它的实现部分分离，使它们都可以独立地变化
    /// </summary>
    public class 桥接模式
    {
        [Fact]
        public void Test()
        {
            HandsetBrand brand;

            brand = new HandsetBrandN();
            brand.SetHandsetSoft(new HandsetGame());
            brand.Run();
            brand.SetHandsetSoft(new HandsetAddressList());
            brand.Run();

            brand = new HandsetBrandM();
            brand.SetHandsetSoft(new HandsetGame());
            brand.Run();
            brand.SetHandsetSoft(new HandsetAddressList());
            brand.Run();
        }

        /// <summary>
        /// 手机软件
        /// </summary>
        abstract class HandsetSoft
        {
            public abstract void Run();
        }

        /// <summary>
        /// 手机游戏
        /// </summary>
        class HandsetGame : HandsetSoft
        {
            public override void Run()
            {
                Console.WriteLine("运行手机游戏");
            }
        }
        /// <summary>
        /// 手机通讯录
        /// </summary>
        class HandsetAddressList : HandsetSoft
        {
            public override void Run()
            {
                Console.WriteLine("运行手机通讯录");
            }
        }

        /// <summary>
        /// 手机品牌
        /// </summary>
        abstract class HandsetBrand
        {
            protected HandsetSoft _handsetSoft;
            public void SetHandsetSoft(HandsetSoft handsetSoft)
            {
                _handsetSoft = handsetSoft;
            }
            public abstract void Run();
        }

        /// <summary>
        /// 品牌N
        /// </summary>
        class HandsetBrandN : HandsetBrand
        {
            public override void Run()
            {
                _handsetSoft.Run();
            }
        }

        /// <summary>
        /// 品牌M
        /// </summary>
        class HandsetBrandM : HandsetBrand
        {
            public override void Run()
            {
                _handsetSoft.Run();
            }
        }
    }
}
