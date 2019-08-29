namespace Core.Common.Entity
{
    /// <summary>
    /// 数据模型接口
    /// </summary>
    public interface IEntity<out TKey>
    {
        /// <summary>
        /// 获取 实体唯一标识，主键
        /// </summary>
        TKey Id { get; }
    }
}
