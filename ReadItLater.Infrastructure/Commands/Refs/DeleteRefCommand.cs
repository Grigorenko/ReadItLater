using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Refs
{
    public class DeleteRefCommand : ICommand
    {
        public DeleteRefCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class DeleteRefCommandHandler : AsyncCommandHandlerBase<DeleteRefCommand>
        {
            private const string spDeleteRef = "DeleteRef";

            public DeleteRefCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(DeleteRefCommand command, CancellationToken token = default)
            {
                await dapperContext.ExecuteProcedureAsync(spDeleteRef, new { command.Id }, token);

                return Result.Success();
            }
        }
    }
}
