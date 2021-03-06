using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Data.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using ReadItLater.Web.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Infrastructure.Queries.User;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Infrastructure.Commands.User;

namespace ReadItLater.Web.Server.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly AuthenticationConfiguration config;

        public AuthController(
            IMessages messages,
            IOptions<AuthenticationConfiguration> options)
            : base(messages)
        {
            config = options.Value;
        }

        [HttpGet("currentuserinfo")]
        public Task<CurrentUser> CurrentUserInfo()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Task.FromResult(CurrentUser.Unauthorized);

            var user = new CurrentUser
            {
                IsAuthenticated = User.Identity!.IsAuthenticated,
                UserName = User.FindFirstValue(ClaimTypes.Name),
                Claims = User.Claims.ToDictionary(p => p.Type, p => p.Value),
                Token = AuthenticationHelper.GenerateJwtToken(User.FindFirstValue(ClaimTypes.NameIdentifier), config.Secret!, config.JwtTokenExpirationTimeInMinutes)
            };

            return Task.FromResult(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<CurrentUser>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await messages.DispatchAsync(new GetUserByCredentialsQuery(loginRequest.UserName!, loginRequest.Password!), HttpContext.RequestAborted);

            if (result.IsFailure)
                return BadRequest(result);

            return Ok(Result.Success(
                new CurrentUser
                {
                    IsAuthenticated = true,
                    //UserName = User.FindFirstValue(ClaimTypes.Name),
                    Claims = GetUserClaims(result.Value!).ToDictionary(p => p.Type, p => p.Value),
                    Token = AuthenticationHelper.GenerateJwtToken(result.Value!.Id.ToString(), config.Secret!, config.JwtTokenExpirationTimeInMinutes)
                }));
        }

        [HttpPost("register")]
        public async Task Register(RegisterRequest registerRequest) =>
            await ExecuteCommand(new CreateUserCommand(registerRequest.UserName!, registerRequest.Password!));

        [HttpPost("logout")]
        public Task Logout()
        {
            //delete current token
            SignOut(JwtBearerDefaults.AuthenticationScheme);
            return Task.CompletedTask;
        }

        private IEnumerable<Claim> GetUserClaims(UserProjection user)
        {
            return new Dictionary<string, string>
            {
                [ClaimTypes.NameIdentifier] = user.Id.ToString(),
                [ClaimTypes.Name] = user.Name
            }
            .Select(p => new Claim(p.Key, p.Value));
        }
    }
}
