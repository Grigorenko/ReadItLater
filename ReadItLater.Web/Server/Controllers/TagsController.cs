using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Infrastructure.Queries.Tags;
using ReadItLater.Core.Infrastructure.Utils;

namespace ReadItLater.Web.Server.Controllers
{
    public class TagsController : BaseController
    {
        public TagsController(IMessages messages) : base(messages)
        {
        }

        [HttpGet("autofill")]
        public async Task<ActionResult<IEnumerable<TagProjection>>> GetList([FromQuery] string key) =>
            await ExecuteQuery(new GetTagsListQuery(key));
    }
}
