using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.Tags
{
    public class GetTagsListQuery : IQuery<IEnumerable<TagProjection>>
    {
        public GetTagsListQuery(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetTagsListQueryHandler : AsyncQueryHandlerBase<GetTagsListQuery, IEnumerable<TagProjection>>
        {
            private const string spSelectTagsByName = "SelectTagsByName";

            public GetTagsListQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<TagProjection>>> HandleAsync(GetTagsListQuery query, CancellationToken token = default)
            {
                var tags = await dapperContext.SelectAsync<TagProjection>(spSelectTagsByName, new { name = query.Key }, CommandType.StoredProcedure, token);

                return Result.Success(tags);
            }
        }
    }
}
