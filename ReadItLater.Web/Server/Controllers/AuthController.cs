using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Data.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using ReadItLater.Web.Server.Utils;
using Microsoft.AspNetCore.Authorization;

namespace ReadItLater.Web.Server.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthenticationConfiguration config;

        public AuthController(IOptions<AuthenticationConfiguration> options)
        {
            this.config = options.Value;
        }

        private readonly User ExistUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "iam",
            Pass = "123"
        };
        static bool IsAuth = false;

        [AllowAnonymous]
        [HttpGet("currentuserinfo")]
        public Task<CurrentUser> CurrentUserInfo()
        {
            if (!IsAuth)
                return Task.FromResult(CurrentUser.Unauthorized);

            var user = new CurrentUser
            {
                IsAuthenticated = IsAuth,
                UserName = ExistUser.Name,
                Claims = new Dictionary<string, string>
                {
                    [ClaimsIdentity.DefaultNameClaimType] = ExistUser.Name
                },
                Token = AuthenticationHelper.GenerateJwtToken(ExistUser.Id.ToString(), config.Secret!, config.JwtTokenExpirationTimeInMinutes)
            };

            return Task.FromResult(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public Task Login([FromBody] LoginRequest loginRequest)
        {
            if (ExistUser.Name.Equals(loginRequest.UserName) && ExistUser.Pass.Equals(loginRequest.Password))
                IsAuth = true;

            return Task.CompletedTask;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public Task Register(RegisterRequest registerRequest)
        {
            return Task.CompletedTask;
        }

        [HttpPost("logout")]
        public Task Logout()
        {
            IsAuth = false;
            return Task.CompletedTask;
        }
    }
}
