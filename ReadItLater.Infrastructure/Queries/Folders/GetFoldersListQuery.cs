using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.Folders
{
    public class GetFoldersListQuery : IQuery<IEnumerable<FolderListItemProjection>>
    {

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetFoldersListQueryHandler : AsyncQueryHandlerBase<GetFoldersListQuery, IEnumerable<FolderListItemProjection>>
        {
            private const string spSelectFolderListItems = "SelectFolderListItems";

            public GetFoldersListQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<FolderListItemProjection>>> HandleAsync(GetFoldersListQuery query, CancellationToken token = default)
            {
                var folders = await dapperContext.SelectAsync<FolderListItemProjection>(spSelectFolderListItems, new { userId = currentUserId }, cancellationToken: token);
                var root = folders
                    .Where(f => f.ParentId is null)
                    .OrderBy(f => f.Order)
                    .ToList();

                foreach (var item in root)
                {
                    var nested = folders
                        .Where(f => f.ParentId == item.Id)
                        .OrderBy(f => f.Order)
                        .ToList();

                    item.RefsCount += nested.Sum(n => n.RefsCount);
                    item.Folders = nested;
                }

                return Result.Success(root as IEnumerable<FolderListItemProjection>);
            }
        }
    }
}
