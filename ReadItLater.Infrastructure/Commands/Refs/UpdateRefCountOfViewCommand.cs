using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Refs
{
    public class UpdateRefCountOfViewCommand : ICommand
    {
        public UpdateRefCountOfViewCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class UpdateRefCountOfViewCommandHandler : AsyncCommandHandlerBase<UpdateRefCountOfViewCommand>
        {
            private const string spUpdateCountOfView = "UpdateCountOfView";

            public UpdateRefCountOfViewCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(UpdateRefCountOfViewCommand command, CancellationToken token = default)
            {
                await dapperContext.ExecuteProcedureAsync(spUpdateCountOfView, new { refId = command.Id }, token);

                return Result.Success();
            }
        }
    }
}
