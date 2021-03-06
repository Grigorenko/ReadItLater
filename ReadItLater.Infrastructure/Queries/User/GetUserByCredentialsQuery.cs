using ReadItLater.Core;
using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Queries.User
{
    public class GetUserByCredentialsQuery : IQuery<UserProjection>
    {
        public GetUserByCredentialsQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        [AuditLogQuery]
        [DataValidationQuery]
        public class GetUserByCredentialsQueryHandler : AsyncAnonymousQueryHandlerBase<GetUserByCredentialsQuery, UserProjection>
        {
            private const string spSelectUserByCredentials = "SelectUserByCredentials";

            public GetUserByCredentialsQueryHandler(IDapperContext dapperContext)
                : base(dapperContext)
            {
            }

            public override async Task<Result<UserProjection>> HandleAsync(GetUserByCredentialsQuery query, CancellationToken token = default)
            {
                var password = MD5Helper.Hash(query.Password);
                var user = await dapperContext.SingleOrDefaultAsync<UserProjection>(spSelectUserByCredentials, new { query.Username, password }, CommandType.StoredProcedure, token);

                if (user is null)
                    return Result.Failure<UserProjection, NotFoundException>(NotFoundException.Make(query, q => query.Username, q => query.Password));

                return Result.Success(user);
            }
        }
    }
}
