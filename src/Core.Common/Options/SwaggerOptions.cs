namespace Core.Common.Options
{
    /// <summary>
    /// Swagger选项
    /// </summary>
    public class SwaggerOption
    {
        /// <summary>
        /// 获取或设置 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置 版本
        /// </summary>
        public string Version { get; set; }      

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
