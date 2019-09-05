using Core.Common.Entity;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class IdentityRole : EntityBase<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
