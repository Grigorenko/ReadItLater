using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Infrastructure.Commands.Folders;
using ReadItLater.Data.Dtos.Folder;
using ReadItLater.Infrastructure.Queries.Folders;

namespace ReadItLater.Web.Server.Controllers
{
    public class FoldersController : BaseController
    {
        public FoldersController(IMessages messages) : base(messages)
        {
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<FolderListItemProjection>>> GetList() =>
            await ExecuteQuery(new GetFoldersListQuery());

        [HttpGet("{id:guid}/tags")]
        public async Task<ActionResult<IEnumerable<TagListItemProjection>>> GetTags(Guid id) =>
            await ExecuteQuery(new GetTagsByFolderQuery(id));

        [HttpGet("{id:guid}/breadcrumbs")]
        public async Task<ActionResult<IEnumerable<BreadcrumbProjection>>> GetBreadcrumbs(Guid id) =>
            await ExecuteQuery(new GetBreadcrumbsQuery(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFolderDto folder) =>
            await ExecuteCommand(new CreateFolderCommand(folder.Id, folder.ParentId, folder.Name!));

        [HttpPatch("{id:guid}/name/{name}")]
        public async Task<IActionResult> Rename([FromRoute] Guid id, [FromRoute] string name) =>
            await ExecuteCommand(new ChangeFolderNameCommand(id, name));

        [HttpPatch("{id:guid}/moveup")]
        public async Task<IActionResult> MoveUp([FromRoute] Guid id) =>
            await ExecuteCommand(new MoveUpFolderCommand(id));

        [HttpPatch("{id:guid}/movedown")]
        public async Task<IActionResult> MoveDown([FromRoute] Guid id) =>
            await ExecuteCommand(new MoveDownFolderCommand(id));

        [HttpDelete("{id:guid}")]
        public async Task Delete([FromRoute] Guid id) =>
            await ExecuteCommand(new DeleteFolderCommand(id));
    }
}
