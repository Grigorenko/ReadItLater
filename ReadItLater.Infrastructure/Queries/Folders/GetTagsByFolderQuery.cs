using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.Folders
{
    public class GetTagsByFolderQuery : IQuery<IEnumerable<TagListItemProjection>>
    {
        public GetTagsByFolderQuery(Guid folderId)
        {
            FolderId = folderId;
        }

        public Guid FolderId { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetTagsByFolderQueryHandler : AsyncQueryHandlerBase<GetTagsByFolderQuery, IEnumerable<TagListItemProjection>>
        {
            private const string spGetTagsByFolder = "GetTagsByFolder";

            public GetTagsByFolderQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<TagListItemProjection>>> HandleAsync(GetTagsByFolderQuery query, CancellationToken token = default)
            {
                var tags = await dapperContext.SelectAsync<TagListItemProjection>(spGetTagsByFolder, new { query.FolderId }, cancellationToken: token);

                return Result.Success(tags);
            }
        }
    }
}
