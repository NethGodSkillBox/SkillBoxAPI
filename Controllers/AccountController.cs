using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillBoxAPI.Context;
using SkillBoxAPI.Interfaces;
using SkillBoxAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SkillBoxAPI.Controllers
{
    public class AccountController : Controller
    {
        private IReq reqData;
        public AccountController(IReq ReqData)
        {
            this.reqData = ReqData;
        }

        [HttpPost]
        [Route("token")]
        public IActionResult Token(string data)
        {
            string username = data.Split(',')[0].Split('=')[1];
            string password = data.Split(',')[1].Split('=')[1];

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", encodedJwt,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60)
            });
            return Json(response);
        }

        [HttpGet]
        [Route("token")]
        public IActionResult Token()
        {
            string response = $"{{\"123\":\"213\"}}";
            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var check = reqData.Check(username, password).Result;
            if (check)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "Admin"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
