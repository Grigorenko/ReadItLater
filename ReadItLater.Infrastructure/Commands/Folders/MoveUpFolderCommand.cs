using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Folders
{
    public class MoveUpFolderCommand : ICommand
    {
        public MoveUpFolderCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class MoveUpFolderCommandHandler : AsyncCommandHandlerBase<MoveUpFolderCommand>
        {
            private const string spMoveUpFolder = "MoveUpFolder";

            public MoveUpFolderCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider) 
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(MoveUpFolderCommand command, CancellationToken token = default)
            {
                await dapperContext.ExecuteProcedureAsync(spMoveUpFolder, new { command.Id }, token);

                return Result.Success();
            }
        }
    }
}
