using Core.Common.Entity;
using Core.Models.Enum.Identity;

namespace Core.Models.Identity.Entities
{
    /// <summary>
    /// 功能表
    /// </summary>
    public class IdentityFunction : EntityBase<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public EnumFunctionAccessType AccessType { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 控制器的功能名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string Methods { get; set; }

        /// <summary>
        /// 请求链接
        /// </summary>
        public string Url { get; set; }
    }
}
