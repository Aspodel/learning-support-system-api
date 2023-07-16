using LearningSupportSystemAPI.Models;
using LearningSupportSystemAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace LearningSupportSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region [Fields]
        private readonly UserManager _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptionsMonitor<JwtTokenConfig> _tokenConfigOptionsAccessor;
        #endregion

        #region [Ctor]
        public AuthController(UserManager userManager, SignInManager<User> signInManager, IOptionsMonitor<JwtTokenConfig> tokenConfigOptionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigOptionsAccessor = tokenConfigOptionsAccessor;
        }
        #endregion

        #region [POST]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user is null)
                return new BadRequestObjectResult(new { message = "Username or password is incorrect" });

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!passwordCheck.Succeeded)
                return new BadRequestObjectResult(new { message = "Username or password is incorrect" });

            var tokenConfig = _tokenConfigOptionsAccessor.CurrentValue;
            var requestAt = DateTime.UtcNow;
            var expiresIn = requestAt + tokenConfig.ExpiresIn;
            var token = await GenerateToken(user, tokenConfig, expiresIn);
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                State = 200,
                Msg = "OK",
                Lang = "en",
                Data = new
                {
                    requestAt,
                    expiresIn = tokenConfig.ExpiresIn.TotalSeconds,
                    tokenType = tokenConfig.TokenType,
                    accessToken = token,
                    refresh_token,
                    user.Id,
                    user.IdCard,
                    user.Email,
                    roles
                }
            });
        }
        #endregion

        private async Task<string> GenerateToken(User user, JwtTokenConfig tokenConfig, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var roles = await _userManager.GetRolesAsync(user);

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.UserName!, "TokenAuth"),
                new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), new Claim("id", user.Id.ToString()) }
                .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)))
            );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Subject = identity,
                Expires = expires
            });

            return handler.WriteToken(securityToken);
        }
    }
}
