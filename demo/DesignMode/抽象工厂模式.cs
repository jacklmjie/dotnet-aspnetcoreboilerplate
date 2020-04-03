using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 提供一个创建一系列相关或依赖对象的的接口
    /// 而无需指定它们具体的类
    /// </summary>
    public class 抽象工厂模式
    {
        [Fact]
        public void Test()
        {
            User user = new User();

            IFactory factory = new SqlserverFactory();
            IUser iu = factory.CreateUser();

            iu.Insert(user);
            iu.GetUser(1);
        }
        /// <summary>
        /// 定义一个创建访问User表对象的抽象的工厂接口
        /// </summary>
        interface IFactory
        {
            IUser CreateUser();
        }
        /// <summary>
        /// 实例化SqlserverUser
        /// </summary>
        class SqlserverFactory : IFactory
        {
            public IUser CreateUser()
            {
                return new SqlserverUser();
            }
        }
        /// <summary>
        /// 实例化AccessUser
        /// </summary>
        class AccessFactory : IFactory
        {
            public IUser CreateUser()
            {
                return new AccessUser();
            }
        }
        /// <summary>
        /// 用户客户端访问，解除与具体数据库访问的耦合
        /// </summary>
        interface IUser
        {
            void Insert(User user);
            User GetUser(int id);
        }
        /// <summary>
        /// sqlserver具体类
        /// </summary>
        class SqlserverUser : IUser
        {
            public void Insert(User user)
            {
                Console.WriteLine("sqlserver 新增一条数据");
            }

            public User GetUser(int id)
            {
                Console.WriteLine("sqlserver 获取一条数据");
                return null;
            }
        }
        /// <summary>
        /// Access具体类
        /// </summary>
        class AccessUser : IUser
        {
            public User GetUser(int id)
            {
                Console.WriteLine("Access 获取一条数据");
                return null;
            }

            public void Insert(User user)
            {
                Console.WriteLine("Access 新增一条数据");
            }
        }

        class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
