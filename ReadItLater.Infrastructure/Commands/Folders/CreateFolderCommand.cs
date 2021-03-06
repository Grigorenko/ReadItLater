using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Folders
{
    public class CreateFolderCommand : ICommand
    {
        public CreateFolderCommand(Guid? id, Guid? parentId, string name)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
        }

        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class CreateFolderCommandHandler : AsyncCommandHandlerBase<CreateFolderCommand>
        {
            private const string spCreateFolder = "CreateFolder";
            
            public CreateFolderCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider) 
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(CreateFolderCommand command, CancellationToken token = default)
            {
                var folder = new Folder
                {
                    Id = command.Id ?? Guid.NewGuid(),
                    ParentId = command.ParentId,
                    Name = command.Name
                };

                if (folder.Id == default)
                    return Result.Failure(nameof(command.Id), "Id prop must not be default.");

                await dapperContext.ExecuteProcedureAsync(spCreateFolder, new { userId = userProvider.CurrentUserId, folder = folder.ToUdt() }, token);

                return Result.Success();
            }
        }
    }
}
