namespace Core.Common.Entity
{
    /// <summary>
    /// 定义删除状态
    /// </summary>
    public interface IDeleteStatus
    {
        /// <summary>
        /// 0默认正常状态 1删除状态
        /// </summary>
        int Status { get; set; }
    }
}
