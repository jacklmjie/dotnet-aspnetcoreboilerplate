namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 用户-角色表
    /// </summary>
    public class IdentityUserRole
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
    }
}
