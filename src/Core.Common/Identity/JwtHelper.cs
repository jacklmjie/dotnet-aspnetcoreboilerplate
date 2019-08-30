using System;
using System.Text;
using Core.Common.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Common.Identity
{
    /// <summary>
    /// Jwt辅助操作类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        public static string CreateToken(Claim[] claims, JwtOption jwtOption)
        {
            string secret = jwtOption.Secret;
            if (secret == null)
            {
                throw new APIException("500", "创建JwtToken时Secret为空");
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            DateTime now = DateTime.Now;
            double days = Math.Abs(jwtOption.ExpireDays);
            DateTime expires = now.AddDays(days);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtOption.Audience,
                Issuer = jwtOption.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
