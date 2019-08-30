namespace Core.Models.Identity.Dtos
{
    /// <summary>
    /// 刷新Token
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 刷新的Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
