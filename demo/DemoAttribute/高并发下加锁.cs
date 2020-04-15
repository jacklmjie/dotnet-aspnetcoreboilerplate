using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DemoAttribute
{
    public class 高并发下加锁
    {
        [Fact]
        public void Test()
        {

        }

        public class Integer
        {
            public int Money { get; set; }
        }

        /// <summary>
        /// 转账
        /// </summary>
        public class TansferAccount
        {
            private readonly object syncLock = new object();

            private int balance;
            public void transfer(TansferAccount target, Integer transferMoney)
            {
                //lock(syncLock) 这样只对target加了锁，对transferMoney没有加锁
                lock (this)//TODO:这样有效没待确认
                {
                    if (balance >= transferMoney.Money)
                    {
                        balance -= transferMoney.Money;
                        target.balance += transferMoney.Money;
                    }
                }
            }
        }
    }
}
