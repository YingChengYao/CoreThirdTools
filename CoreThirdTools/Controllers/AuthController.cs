using CoreThirdTools.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreThirdTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 初始化token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        public string GenerateTokenAsync(string username, string password)
        {
            //TODO 用户和密码验证
            if (username == "ycy" && password == "123")
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, "123@meowv.com"),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.JWT.Expires))).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var securityToken = new JwtSecurityToken(
                    issuer: AppSettings.JWT.ValidIssuer,
                    audience: AppSettings.JWT.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.JWT.Expires)),
                    signingCredentials: creds);

                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                return token;
            }
            else
            {
                throw new Exception("账号密码错误");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("AuthorizeTest")]
        public string AuthorizeTest()
        {
            return "我是返回结果";
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("AllowAnonymousTest")]
        public string AllowAnonymousTest()
        {
            return "我是返回结果";
        }
    }
}
