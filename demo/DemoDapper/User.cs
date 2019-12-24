using System;
using System.Collections.Generic;

namespace DemoDapper
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        /// <summary>
        /// User变动记录log
        /// 用JsonTypeHandler 自订Mapping逻辑使用、底层逻辑
        /// </summary>
        public List<Log> Logs { get; set; }
    }

    public class Log
    {
        public DateTime Time { get; set; }
        public string Remark { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public string OrderNo { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
