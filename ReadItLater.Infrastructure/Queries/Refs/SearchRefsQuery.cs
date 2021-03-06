using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.Refs
{
    public class SearchRefsQuery : IQuery<IEnumerable<RefProjection>>
    {
        public SearchRefsQuery(Guid? folderId, Guid? tagId, int? offset, int? limit, string? sort, string searchTerm)
        {
            FolderId = folderId;
            TagId = tagId;
            Offset = offset;
            Limit = limit;
            Sort = sort;
            SearchTerm = searchTerm;
        }

        public Guid? FolderId { get; set; }
        public Guid? TagId { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string? Sort { get; set; }
        public string SearchTerm { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class SearchRefsQueryHandler : AsyncQueryHandlerBase<SearchRefsQuery, IEnumerable<RefProjection>>
        {
            private const string spSearchRefs = "SearchRefs";

            public SearchRefsQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<RefProjection>>> HandleAsync(SearchRefsQuery query, CancellationToken token = default)
            {
                var param = new
                {
                    userId = userProvider.CurrentUserId,
                    query.FolderId,
                    query.TagId,
                    offset = query.Offset ?? 0,
                    limit = query.Limit ?? 20,
                    sort = query.Sort?.ToSortUdt(),
                    query.SearchTerm
                };

                var refs = await dapperContext.ExecStoredProcedureWithMapping(
                    spSearchRefs,
                    param,
                    x => x.Id,
                    RefProjectionMapConfig.Action,
                    token);

                return Result.Success(refs);
            }
        }
    }
}
