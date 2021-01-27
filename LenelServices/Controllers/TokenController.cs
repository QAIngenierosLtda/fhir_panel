using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace LenelServices.Controllers
{
    //[Area("WebApi")]
    //[Route("/token")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("/api/Token/Create")]
        public async  Task<IActionResult> Create(string username, string password)
        {
            if (await IsValidUserAndPasswordCombination(username, password))
                return new ObjectResult(GenerateToken(username));
            return BadRequest();
        }

        private async Task<bool> IsValidUserAndPasswordCombination(string username, string password)
        {
            string user = _config.GetSection("JwtConfig:UserToken").Value.ToString();
            string pass = MD5Encrypt(_config.GetSection("JwtConfig:PassToken").Value.ToString());

            if (user == username && pass == MD5Encrypt(password))
                return true;
            return false;
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtConfig:Key").Value.ToString())),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string MD5Encrypt(string password)
        {
            // byte array representation of that string
            byte[] encodedPassword = new System.Text.UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash)
                  // without dashes
                  .Replace("-", string.Empty)
                  // make lowercase
                  .ToLower();

            return encoded;
        }
    }
}