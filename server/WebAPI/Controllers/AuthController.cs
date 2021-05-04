using Domain.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly string Scheme = IdentityConstants.ApplicationScheme;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<UserEntity> _userManager;

        public AuthController(ILogger<AuthController> logger, IMediator mediator, UserManager<UserEntity> userManager) : base(mediator)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            //var getUser = await _userManager.ConfirmEmailAsync(user, token);

            if (user == null)
            {
                // email user about having no acc??? really?
                return Result(CommandResult<string>.Ok("you don't have an account yet"));
            }
            else
            {
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");

                // check what is generated
                var url = Url.Action("LoginCallback", "Auth", new { token = token, email = email }, Request.Scheme);
                return Result(CommandResult<string>.Ok($"visit this link to login: {url}"));
            }
        }

        [HttpGet("logincb")]
        public async Task<IActionResult> LoginCallback([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "passwordless-auth", token);

            if (isValid)
            {
                await _userManager.UpdateSecurityStampAsync(user);

                await HttpContext.SignInAsync(
                    Scheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("sub", user.Id.ToString()) }, Scheme)));

                return Result(CommandResult<string>.Ok("you are now logged in, check dat"));
            }

            return Result(CommandResult<string>.Ok("user appeared to be not valid"));
        }

        [Authorize]
        [HttpGet("ping-auth")]
        public async Task<IActionResult> PingAuth()
        {
            var userClaims = User;
            var userId = userClaims.FindFirstValue("sub");
            var userData = await _userManager.FindByIdAsync(userId);
            return Result(CommandResult<string>.Ok($"ping-auth: pong: {userId}"));
        }

        [HttpGet("ping-not-auth")]
        public async Task<IActionResult> PingNotAuth()
        {
            var userClaims = User;
            var userId = userClaims.FindFirstValue("sub");
            var userData = await _userManager.FindByIdAsync(userId);
            return Result(CommandResult<string>.Ok($"ping-not-auth: pong: {userId}"));
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var userClaims = User;
            var userId = userClaims.FindFirstValue("sub");
            var userData = await _userManager.FindByIdAsync(userId);
            await _userManager.UpdateSecurityStampAsync(userData);
            await HttpContext.SignOutAsync(Scheme);
            return Result(CommandResult<string>.Ok("signed out"));
        }
    }
}
