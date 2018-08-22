using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Core.Data;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private UserManager<ApplicationUser> userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6206",
                    audience: "http://localhost:6206",
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                await addToken(token, user.Id);
                HttpContext.Session.SetString("token", token.EncodedHeader);
                HttpContext.Session.SetString("userId", user.Id);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        public async Task<IActionResult> addToken(JwtSecurityToken token, string id)
        {
            TokenDatabaseContext table = new TokenDatabaseContext();
            AspNetUserTokens tokenUser = new AspNetUserTokens();
            tokenUser.UserId = id;
            tokenUser.Value = token.EncodedHeader;
            await table.AspNetUserTokens.AddAsync(tokenUser);
            await table.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] UserModel model)
        {
            var usersName = await userManager.FindByNameAsync(model.Username);
            if (usersName == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                await userManager.CreateAsync(user, model.Password);

                var claims = new[]
{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                    issuer: "http://localhost:57765",
                    audience: "http://localhost:57765",
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                await addToken(token, user.Id);
                HttpContext.Session.SetString("token", token.EncodedHeader);
                HttpContext.Session.SetString("userId", user.Id);
            }
            return Ok();
        }
    }
}