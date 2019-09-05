using Core.IContract;
using Core.Common.Options;
using Core.Common.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Models.Identity.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Core.API.Controllers
{
    /// <summary>
    /// 网站-认证
    /// </summary>
    public class IdentityController : ApiController
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly JwtOption _option;
        private readonly IIdentityContract _identityContract;
        public IdentityController(
            ILogger<IdentityController> logger,
            IOptions<JwtOption> option,
            IIdentityContract identityContract)
        {
            _logger = logger;
            _option = option.Value;
            _identityContract = identityContract;
        }

        /// <summary>
        /// JWT登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> PostLogin(LoginDto dto)
        {
            var user = await _identityContract.GetUserByName(dto.UserName);
            if (user == null)
            {
                ModelState.AddModelError($"用户{dto.UserName}", "不存在");
                return BadRequest(ModelState);
            }
            if (!user.Password.Equals(dto.Password))
            {
                ModelState.AddModelError("密码", "错误");
                return BadRequest(ModelState);
            }
            string token = CreateJwtToken(user);
            return Ok(token);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("user-info")]
        [Authorize]
        //[ApiConventionMethod(typeof(MyAppConventions),
        //                     nameof(MyAppConventions.Get))]
        public ActionResult GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(claimsIdentity.Claims.ToList().Select(r => new { r.Type, r.Value }));
        }

        /// <summary>
        /// 生成Jwt的Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateJwtToken(IdentityUser user)
        {
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("NickName", user.NickName)
            };

            var token = JwtHelper.CreateToken(claims, _option);
            return token;
        }
    }
}