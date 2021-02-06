using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Data.EF.Interfaces;

namespace ReadItLater.Web.Server.Controllers
{
    public class FoldersController : BaseController
    {
        private readonly IDapperContext dapperContext;
        private readonly IDapperContext<TagListItemProjection> tagDapperContext;
        private readonly IDapperContext<FolderListItemProjection> folderDapperContext;
        private readonly IDapperContext<BreadcrumbProjection> breadcrumbDapperContext;

        public FoldersController(
            IDapperContext dapperContext,
            IDapperContext<TagListItemProjection> tagDapperContext,
            IDapperContext<FolderListItemProjection> folderDapperContext,
            IDapperContext<BreadcrumbProjection> breadcrumbDapperContext)
        {
            this.dapperContext = dapperContext;
            this.tagDapperContext = tagDapperContext;
            this.folderDapperContext = folderDapperContext;
            this.breadcrumbDapperContext = breadcrumbDapperContext;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<FolderListItemProjection>> GetList(CancellationToken cancellationToken)
        {
            var folders = await folderDapperContext.SelectAsync("SelectFolderListItems", commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
            var root = folders.Where(f => f.ParentId is null).OrderBy(f => f.Order).ToList();

            foreach (var item in root)
            {
                var nested = folders.Where(f => f.ParentId == item.Id).OrderBy(f => f.Order).ToList();

                item.RefsCount += nested.Sum(n => n.RefsCount);
                item.Folders = nested;
            }

            return root;
        }

        [HttpGet("{id:guid}/tags")]
        public async Task<IEnumerable<TagListItemProjection>> GetTags(Guid id, CancellationToken cancellationToken)
        {
            var tags = await tagDapperContext.SelectAsync("GetTagsByFolder", new { folderId = id }, System.Data.CommandType.StoredProcedure, cancellationToken);

            return tags;
        }

        [HttpGet("{id:guid}/breadcrumbs")]
        public async Task<IEnumerable<BreadcrumbProjection>> GetBreadcrumbs(Guid id, CancellationToken cancellationToken)
        {
            var breadcrumbs = await breadcrumbDapperContext.SelectAsync("GetBreadcrumbs", new { id }, System.Data.CommandType.StoredProcedure, cancellationToken);

            return breadcrumbs;
        }

        [HttpPost]
        public async Task Create([FromBody]Folder folder, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("CreateFolder", new { folder = folder.ToUdt() }, cancellationToken);
        }

        [HttpPatch("{id:guid}/name/{name}")]
        public async Task Rename([FromRoute] Guid id, [FromRoute] string name, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("RenameFolder", new { id, name }, cancellationToken);
        }

        [HttpPatch("{id:guid}/moveup")]
        public async Task MoveUp([FromRoute] Guid id,  CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("MoveUpFolder", new { id }, cancellationToken);
        }

        [HttpPatch("{id:guid}/movedown")]
        public async Task MoveDown([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("MoveDownFolder", new { id }, cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("DeleteFolder", new { id }, cancellationToken);
        }
    }
}
