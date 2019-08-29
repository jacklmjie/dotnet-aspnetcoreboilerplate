namespace Core.Common.Entity
{
    /// <summary>
    /// 定义创建审计信息
    /// </summary>
    public interface ICreationAudited<TUserKey> : ICreatedTime
        where TUserKey : struct
    {
        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        TUserKey? CreatorId { get; set; }
    }
}
