using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReadItLater.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using ReadItLater.Infrastructure.Queries.Refs;
using ReadItLater.Infrastructure.Commands.Refs;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Core.Infrastructure;

namespace ReadItLater.Web.Server.Controllers
{
    public class RefsController : BaseController
    {
        public RefsController(IMessages messages) : base(messages)
        {
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RefProjection>> GetList([FromRoute]Guid id) =>
            await ExecuteQuery(new GetRefByIdQuery(id));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefProjection>>> GetList(Guid? folderId, Guid? tagId, int offset, int limit, string? sort) =>
            await ExecuteQuery(new GetRefsListQuery(folderId, tagId, offset, limit, sort));

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RefProjection>>> Search(Guid? folderId, Guid? tagId, string searchTerm, int offset, int limit, string? sort) =>
            await ExecuteQuery(new SearchRefsQuery(folderId, tagId, offset, limit, sort, searchTerm));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ref @ref) =>
            await ExecuteCommand(new CreateRefCommand(
                @ref.Id,
                @ref.FolderId,
                @ref.Title,
                @ref.Url,
                @ref.Image,
                @ref.Priority,
                @ref.Note,
                @ref.Tags?.Select(t => t.Name)?.ToArray() ?? new string[0]
            ));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Ref @ref) =>
            await ExecuteCommand(new UpdateRefCommand(
                @ref.Id,
                @ref.FolderId,
                @ref.Title,
                @ref.Url,
                @ref.Image,
                @ref.Priority,
                @ref.Note,
                @ref.Tags?.Select(t => t.Name)?.ToArray() ?? new string[0]
            ));

        [HttpPatch("{refId:guid}")]
        public async Task<IActionResult> UpdateCountOfView([FromRoute] Guid refId) =>
            await ExecuteCommand(new UpdateRefCountOfViewCommand(refId));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) =>
            await ExecuteCommand(new DeleteRefCommand(id));
    }
}
