using Core.Common.Drawing;
using Core.IContract.Service;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Core.API.Controllers
{
    /// <summary>
    /// 网站-通用
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IVerifyCodeService _verifyCodeService;

        public CommonController(
            IVerifyCodeService verifyCodeService)
        {
            _verifyCodeService = verifyCodeService;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>验证码图片文件</returns>
        [HttpGet]
        public string VerifyCode()
        {
            ValidateCoder coder = new ValidateCoder()
            {
                RandomColor = true,
                RandomItalic = true,
                RandomLineCount = 7,
                RandomPointPercent = 10,
                RandomPosition = true
            };
            Bitmap bitmap = coder.CreateImage(4, out string code);
            _verifyCodeService.SetCode(code, out string id);
            return _verifyCodeService.GetImageString(bitmap, id);
        }

        /// <summary>
        /// 验证验证码的有效性
        /// </summary>
        /// <param name="code">验证码字符串</param>
        /// <param name="id">验证码编号</param>
        /// <returns>是否无效</returns>
        [HttpGet]
        public bool CheckVerifyCode(string code, string id)
        {
            return _verifyCodeService.CheckCode(code, id, false);
        }
    }
}
