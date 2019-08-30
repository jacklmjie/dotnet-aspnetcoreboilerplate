using Core.Common.Entity;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Identity.Dtos
{
    /// <summary>
    /// 登录信息Dto
    /// </summary>
    public class LoginDto : IInputDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
