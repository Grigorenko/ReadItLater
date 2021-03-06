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
    public class GetRefsListQuery : IQuery<IEnumerable<RefProjection>>
    {
        public GetRefsListQuery(Guid? folderId, Guid? tagId, int? offset, int? limit, string? sort)
        {
            FolderId = folderId;
            TagId = tagId;
            Offset = offset;
            Limit = limit;
            Sort = sort;
        }

        public Guid? FolderId { get; set; }
        public Guid? TagId { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string? Sort { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetRefsListQueryHandler : AsyncQueryHandlerBase<GetRefsListQuery, IEnumerable<RefProjection>>
        {
            private const string spSelectRefs = "SelectRefs";

            public GetRefsListQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<RefProjection>>> HandleAsync(GetRefsListQuery query, CancellationToken token = default)
            {
                var param = new
                {
                    userId = userProvider.CurrentUserId,
                    query.FolderId,
                    query.TagId,
                    offset = query.Offset ?? 0,
                    limit = query.Limit ?? 20,
                    sort = query.Sort.ToSortUdt()
                };

                var refs = await dapperContext.ExecStoredProcedureWithMapping(
                    spSelectRefs,
                    param,
                    x => x.Id,
                    RefProjectionMapConfig.Action,
                    token);

                return Result.Success(refs);
            }
        }
    }
}
