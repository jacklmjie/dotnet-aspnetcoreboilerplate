using System;

namespace Core.Common.Entity
{
    /// <summary>
    /// 定义更新审计的信息
    /// </summary>
    public interface IUpdateAudited<TUserKey> where TUserKey : struct
    {
        /// <summary>
        /// 获取或设置 更新者编号
        /// </summary>
        TUserKey? LastUpdaterId { get; set; }

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }
    }
}
