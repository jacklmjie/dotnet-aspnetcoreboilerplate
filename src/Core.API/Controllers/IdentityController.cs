using System;
using Core.Common;
using Core.IContract;
using Core.Common.Options;
using Core.Common.Identity;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Models.Identity.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.Identity.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly JwtOption _jwtOption;
        private readonly IIdentityContract _identityContract;
        private readonly IMemoryCache _cache;
        public IdentityController(ILogger<IdentityController> logger,
            IOptions<JwtOption> jwtOption,
            IIdentityContract identityContract,
            IMemoryCache cache)
        {
            _logger = logger;
            _jwtOption = jwtOption.Value;
            _identityContract = identityContract;
            _cache = cache;
        }

        /// <summary>
        /// JWT登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("login"), HttpPost]
        [Description("用户登录")]
        public async Task<ResponseMessage> Login(LoginDto dto)
        {
            var response = new ResponseMessage(false);
            var user = await _identityContract.GetUserByName(dto.UserName);
            if (user == null)
            {
                response.Message = "用户名不存在";
                return response;
            }
            if (!user.Password.Equals(dto.Password))
            {
                response.Message = "密码错误";
                return response;
            }
            string token = CreateJwtToken(user);
            return new ResponseMessage()
            {
                Body = token
            };
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("refresh-token"), HttpPost]
        public async Task<ResponseMessage> RefreshToken([FromBody]RefreshTokenDto dto)
        {
            var response = new ResponseMessage(false);
            if (!_cache.TryGetValue(dto.RefreshToken, out string userName))
            {
                response.Message = "Invalid refreshtoken. ";
                return response;
            }
            if (!dto.UserName.Equals(userName))
            {
                response.Message = "Invalid userName.";
                return response;
            }
            var user = await _identityContract.GetUserByName(dto.UserName);
            if (user == null)
            {
                response.Message = "用户名不存在";
                return response;
            }
            string token = CreateJwtToken(user);
            if (!string.IsNullOrEmpty(token))
            {
                _cache.Remove(dto.RefreshToken);
            }
            return new ResponseMessage()
            {
                Body = token
            };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("userinfo"), HttpPost]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(claimsIdentity.Claims.ToList().Select(r => new { r.Type, r.Value }));
        }

        /// <summary>
        /// 生成Jwt的Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateJwtToken(User user)
        {
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("NickName", user.NickName)
            };

            var refreshToken = Guid.NewGuid().ToString();
            _cache.Set(refreshToken, user.UserName, _jwtOption.RefreshValidFor);

            var token = JwtHelper.CreateToken(claims, _jwtOption);
            var response = new
            {
                auth_token = token,
                refresh_token = refreshToken,
                expires_in = (int)_jwtOption.ValidFor.TotalSeconds,
                token_type = "Bearer"
            };
            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}