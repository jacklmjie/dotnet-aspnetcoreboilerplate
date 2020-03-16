using Core.Common.Entity;
using System;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 用户登录日志表
    /// </summary>
    public class IdentityUserLoginLog : EntityBase<long>, ICreatedTime
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override long Id { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 获取或设置 登录IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获取或设置 用户代理头
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 退出时间
        /// </summary>
        public DateTime? LogoutTime { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
