using System;

namespace Core.Common.Entity
{
    /// <summary>
    /// 定义创建时间
    /// </summary>
    public interface ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }
    }
}
