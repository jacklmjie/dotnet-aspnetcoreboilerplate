namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 角色-权限表
    /// </summary>
    public class IdentityRoleFunction
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public int FunctionId { get; set; }
    }
}
