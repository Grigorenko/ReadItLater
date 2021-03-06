using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Folders
{
    public class DeleteFolderCommand : ICommand
    {
        public DeleteFolderCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class DeleteFolderCommandHandler : AsyncCommandHandlerBase<DeleteFolderCommand>
        {
            private const string spDeleteFolder = "DeleteFolder";

            public DeleteFolderCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(DeleteFolderCommand command, CancellationToken token = default)
            {
                await dapperContext.ExecuteProcedureAsync(spDeleteFolder, new { userId = currentUserId, command.Id }, token);

                return Result.Success();
            }
        }
    }
}
