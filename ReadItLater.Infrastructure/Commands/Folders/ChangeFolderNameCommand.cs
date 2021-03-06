using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Folders
{
    public class ChangeFolderNameCommand : ICommand
    {
        public ChangeFolderNameCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class ChangeFolderNameCommandHandler : AsyncCommandHandlerBase<ChangeFolderNameCommand>
        {
            private const string spRenameFolder = "RenameFolder";

            public ChangeFolderNameCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider) 
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(ChangeFolderNameCommand command, CancellationToken token = default)
            {
                var param = new
                {
                    userId = userProvider.CurrentUserId,
                    command.Id,
                    command.Name
                };

                await dapperContext.ExecuteProcedureAsync(spRenameFolder, param, token);

                return Result.Success();
            }
        }
    }
}
