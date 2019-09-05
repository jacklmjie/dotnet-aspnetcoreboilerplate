using Core.Common.Entity;
using Dapper;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class IdentityUser : EntityBase<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
