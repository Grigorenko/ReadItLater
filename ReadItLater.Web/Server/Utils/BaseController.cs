using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Utils;
using System.Threading.Tasks;

namespace ReadItLater.Web.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : Controller
    {
        protected IMessages messages;

        public BaseController(IMessages messages)
        {
            this.messages = messages;
        }

        protected async Task<ActionResult<TResult>> ExecuteQuery<TResult>(IQuery<TResult> query)
            where TResult : class
        {
            Result<TResult> result = await this.messages.DispatchAsync(query, HttpContext.RequestAborted);

            return FromResult(result);
        }

        protected async Task<IActionResult> ExecuteCommand<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            Result result = await this.messages.DispatchAsync<TCommand>(command, HttpContext.RequestAborted);

            return FromResult(result);
        }

        protected IActionResult FromResult(Result result) =>
            result.IsSuccess ? Ok() : (IActionResult)BadRequest(new DefaultFailedResult(result));

        protected ActionResult<TValue> FromResult<TValue>(Result<TValue> result) where TValue : class =>
            result.IsSuccess ? Ok(result) : (ActionResult<TValue>)BadRequest(result);

        protected OkObjectResult Ok<TValue>(Result<TValue> result) where TValue : class =>
            Ok(new DefaultValueResult<TValue>(result));

        protected BadRequestObjectResult BadRequest(IResult result) =>
            BadRequest(new DefaultFailedResult(result));
    }
}
