using Core.Common.Entity;
using System;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 用户详情表
    /// </summary>
    public class UserDetails : EntityBase<long>, ICreatedTime
    {      
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱确认
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 手机号码 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码是否已确认 
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
