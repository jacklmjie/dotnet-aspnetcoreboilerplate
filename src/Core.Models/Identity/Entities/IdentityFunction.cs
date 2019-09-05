using Core.Common.Entity;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class IdentityFunction : EntityBase<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        ///名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
    }
}
