using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Folders
{
    public class MoveDownFolderCommand : ICommand
    {
        public MoveDownFolderCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class MoveDownFolderCommandHandler : AsyncCommandHandlerBase<MoveDownFolderCommand>
        {
            private const string spMoveDownFolder = "MoveDownFolder";

            public MoveDownFolderCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(MoveDownFolderCommand command, CancellationToken token = default)
            {
                await dapperContext.ExecuteProcedureAsync(spMoveDownFolder, new { command.Id }, token);

                return Result.Success();
            }
        }
    }
}
