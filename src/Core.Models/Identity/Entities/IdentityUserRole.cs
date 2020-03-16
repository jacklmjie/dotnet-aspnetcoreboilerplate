using Core.Common.Entity;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 用户-角色表
    /// </summary>
    public class IdentityUserRole : EntityBase<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override long Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}
