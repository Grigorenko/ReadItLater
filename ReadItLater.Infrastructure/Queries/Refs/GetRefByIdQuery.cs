using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.Refs
{
    public class GetRefByIdQuery : IQuery<RefProjection>
    {
        public GetRefByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetFolderByIdQueryHandler : AsyncQueryHandlerBase<GetRefByIdQuery, RefProjection>
        {
            private const string spSelectRefById = "SelectRefById";

            public GetFolderByIdQueryHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result<RefProjection>> HandleAsync(GetRefByIdQuery query, CancellationToken token = default)
            {
                var @ref = await dapperContext.ExecStoredProcedureSingleWithMapping(
                    spSelectRefById,
                    new { query.Id },
                    x => x.Id,
                    RefProjectionMapConfig.Action,
                    token);

                return Result.Success(@ref);
            }
        }
    }
}
