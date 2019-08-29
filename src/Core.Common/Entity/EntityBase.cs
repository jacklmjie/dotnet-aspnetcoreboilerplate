using Core.Common.Data;
using Core.Common.Extensions;
using System;

namespace Core.Common.Entity
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="EntityBase{TKey}"/>类型的新实例
        /// </summary>
        protected EntityBase()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                Id = CombGuid.NewGuid().CastTo<TKey>();
            }
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public virtual TKey Id { get; set; }
    }
}
