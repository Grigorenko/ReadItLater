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
    public class GetBreadcrumbsQuery : IQuery<IEnumerable<BreadcrumbProjection>>
    {
        public GetBreadcrumbsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetBreadcrumbsQueryHandler : AsyncQueryHandlerBase<GetBreadcrumbsQuery, IEnumerable<BreadcrumbProjection>>
        {
            private const string spGetBreadcrumbs = "GetBreadcrumbs";

            public GetBreadcrumbsQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider) 
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<IEnumerable<BreadcrumbProjection>>> HandleAsync(GetBreadcrumbsQuery query, CancellationToken token = default)
            {
                var breadcrumbs = await dapperContext.SelectAsync<BreadcrumbProjection>(spGetBreadcrumbs, new { query.Id }, cancellationToken: token);

                return Result.Success(breadcrumbs);
            }
        }
    }
}
