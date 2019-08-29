namespace Core.API.Options
{
    /// <summary>
    /// JWT身份认证选项
    /// </summary>
    public class JwtOption
    {
        /// <summary>
        /// 获取或设置 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 获取或设置 发行方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 获取或设置 订阅方
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 获取或设置 Token有效期天数
        /// </summary>
        public double ExpireDays { get; set; }
    }
}
