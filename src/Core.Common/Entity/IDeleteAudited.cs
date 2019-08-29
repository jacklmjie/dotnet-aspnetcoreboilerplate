using System;

namespace Core.Common.Entity
{
    /// <summary>
    /// 定义删除审计的信息
    /// </summary>
    public interface IDeleteAudited<TUserKey> where TUserKey : struct
    {
        /// <summary>
        /// 获取或设置 删除者编号
        /// </summary>
        TUserKey? LastDeleterId { get; set; }

        /// <summary>
        /// 获取或设置 最后删除时间
        /// </summary>
        DateTime? LastDeletedTime { get; set; }
    }
}
