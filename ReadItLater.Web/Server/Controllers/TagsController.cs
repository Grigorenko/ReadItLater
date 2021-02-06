using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Data.EF.Interfaces;

namespace ReadItLater.Web.Server.Controllers
{
    public class TagsController : BaseController
    {
        private readonly IDapperContext<TagProjection> tagDapperContext;

        public TagsController(
            IDapperContext<TagProjection> tagDapperContext)
        {
            this.tagDapperContext = tagDapperContext;
        }

        [HttpGet("autofill")]
        public async Task<IEnumerable<TagProjection>> GetList([FromQuery] string key, CancellationToken cancellationToken)
        {
            var tags = await tagDapperContext.SelectAsync("SelectTagsByName", new { name = key }, System.Data.CommandType.StoredProcedure, cancellationToken);

            return tags;
        }
    }
}
