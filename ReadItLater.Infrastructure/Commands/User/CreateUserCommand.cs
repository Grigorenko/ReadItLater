using ReadItLater.Core;
using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.User
{
    public class CreateUserCommand : ICommand
    {
        public CreateUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class CreateUserCommandHandler : AsyncAnonymousCommandHandlerBase<CreateUserCommand>
        {
            private const string spCreateUser = "CreateUser";

            public CreateUserCommandHandler(IDapperContext dapperContext)
                : base(dapperContext)
            {
            }

            public override async Task<Result> HandleAsync(CreateUserCommand command, CancellationToken token = default)
            {
                var password = MD5Helper.Hash(command.Password);

                await dapperContext.ExecuteProcedureAsync(spCreateUser, new { command.Username, password }, token);

                return Result.Success();
            }
        }
    }
}
