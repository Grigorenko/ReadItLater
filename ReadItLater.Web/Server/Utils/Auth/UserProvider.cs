using Microsoft.AspNetCore.Http;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadItLater.Web.Server.Utils.Auth
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor httpContext;

        public UserProvider(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public Guid? CurrentUserId => Guid.Parse("233D6701-8FDA-45CA-AC9B-129FD75C9B9C");
        //{
        //    get
        //    {
        //        if (!httpContext.HttpContext.User.Identity.IsAuthenticated)
        //            throw new Exception();

        //        var value = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        Guid.TryParse(value, out Guid id);

        //        return id;
        //    }
        //}
    }
}
