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
    [ApiController]
    [Route("[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IDapperContext<TagListItemProjection> tagDapperContext;
        private readonly IDapperContext<FolderListItemProjection> folderDapperContext;
        private readonly IDapperContext<BreadcrumbProjection> breadcrumbDapperContext;

        public FoldersController(
            IDapperContext<TagListItemProjection> tagDapperContext,
            IDapperContext<FolderListItemProjection> folderDapperContext,
            IDapperContext<BreadcrumbProjection> breadcrumbDapperContext)
        {
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
    }
}
