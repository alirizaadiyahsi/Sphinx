using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sphinx.Domain;
using Sphinx.Web.Host.AppConsts;
using Sphinx.Web.Host.Authentication;
using Sphinx.Web.Host.Helpers;
using Sphinx.Web.Host.ViewModels;

namespace Sphinx.Web.Host.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtTokenConfiguration> jwtTokenConfiguration)
        {
            _userManager = userManager;
            _jwtTokenConfiguration = jwtTokenConfiguration.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(ErrorHelper.AddErrorsToModelState(result, ModelState));
            }

            return new OkObjectResult("Account created");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userToVerify = await GetClaimsIdentity(loginViewModel.UserName, loginViewModel.Password);
            if (userToVerify == null)
            {
                return Unauthorized();
            }

            var token = new JwtSecurityToken
            (
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                claims: userToVerify.Claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: _jwtTokenConfiguration.SigningCredentials
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null)
            {
                return null;
            }

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(SphinxClaimTypes.ApiUserRole, SphinxClaimValues.ApiAccess)
                });
            }

            return null;
        }
    }
}