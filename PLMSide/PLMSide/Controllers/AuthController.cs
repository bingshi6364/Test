using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("Token")]
        public async Task<object> GetToken([FromBody] TokenRequest request)
        {
            var audienceConfig = _configuration.GetSection("Audience");
            bool result = SHA1(request.account + request.timestamp +audienceConfig["SecretKey"].ToString(), request.sign, Encoding.UTF8);
            if (true)
            {
                var user = await _userRepository.GetUserAndRolesByAccount(request.account);
                string role = user.Roles.FirstOrDefault().RoleNameShort;
                string username = user.name;
                string roleid = String.Join(",", user.Roles.Select(item => item.RoleID));

                // push the user’s name into a claim, so we can identify the user later on.
                var claims = new[]
                {
                   new Claim(ClaimTypes.Name, username),
                   new Claim(ClaimTypes.Role,role),
                   new Claim("RoleID",roleid),
                   new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                   new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                   new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}")
               };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
                /**
                 * Claims (Payload)
                    Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                    iss: The issuer of the token，token 是给谁的
                    sub: The subject of the token，token 主题
                    exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                    iat: Issued At。 token 创建时间， Unix 时间戳格式
                    jti: JWT ID。针对当前 token 的唯一标识
                    除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
                 * */
                var token =  new JwtSecurityToken(
                    issuer: audienceConfig["Issuer"],
                    audience: audienceConfig["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify account and password"+ request.account + request.timestamp + audienceConfig["SecretKey"].ToString()+":"+ request.sign);
        }

        [HttpGet("Username")]
        public async Task<object> GetToken2()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            string ip1 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string ip2= HttpContext.Connection.RemoteIpAddress.ToString();
            string name= System.Net.Dns.GetHostEntry(Request.HttpContext.Connection.RemoteIpAddress).HostName;
            // throw new Exception();
            return Ok(new { name=name,ip1=ip1,ip2=ip2});
        }

        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="shacontent">加密后的编码</param>
        /// <param name="encode">指定加密编码</param>
        /// <returns>返回40位大写字符串</returns>
        public static bool SHA1(string content,string shacontent, System.Text.Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Clear();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result== shacontent;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("SHA1加密出错：" + ex.Message);
                
            }
        }
    }

    public class TokenRequest
    {
        public string account { get; set; }

        public string timestamp { get; set; }

        public string sign { get; set; }
    }

}
