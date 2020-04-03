using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 策略模式封装算法
    /// 封装几乎任何类型的规则
    /// </summary>
    public class 策略模式
    {
        [Fact]
        public void Test()
        {
            CashContext cash = new CashContext("打8折");
            cash.GetResult(100);
        }

        class CashContext
        {
            private CashSuper cs;
            public CashContext(CashSuper cashSuper)
            {
                cs = cashSuper;
            }
            public CashContext(string type)
            {
                switch (type)
                {
                    case "正常收费":
                        cs = new CashNormal();
                        break;
                    case "打8折":
                        cs = new CashRebate(0.8);
                        break;
                }
            }
            public double GetResult(double money)
            {
                return cs.acceptCash(money);
            }
        }

        /// <summary>
        /// 现金收费抽象类
        /// </summary>
        abstract class CashSuper
        {
            public abstract double acceptCash(double money);
        }

        /// <summary>
        /// 正常收费类
        /// </summary>
        class CashNormal : CashSuper
        {
            public override double acceptCash(double money)
            {
                return money;
            }
        }

        /// <summary>
        /// 打折收费类
        /// </summary>
        class CashRebate : CashSuper
        {
            /// <summary>
            /// 折扣率
            /// </summary>
            private double _moneyRebate;
            public CashRebate(double moneyRebate)
            {
                _moneyRebate = moneyRebate;
            }
            public override double acceptCash(double money)
            {
                return money * _moneyRebate;
            }
        }
    }
}
