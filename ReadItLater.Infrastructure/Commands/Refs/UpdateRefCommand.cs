using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Infrastructure.Attributes;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Infrastructure.Commands.Refs
{
    public class UpdateRefCommand : ICommand
    {
        public UpdateRefCommand(Guid id, Guid? folderId, string title, string url, string? image, Priority priority, string? note, string[] tags)
        {
            Id = id;
            FolderId = folderId;
            Title = title;
            Url = url;
            Image = image;
            Priority = priority;
            Note = note;
            Tags = tags;
        }

        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Image { get; set; }
        public Priority Priority { get; set; }
        public string? Note { get; set; }
        public string[] Tags { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class UpdateRefCommandHandler : AsyncCommandHandlerBase<UpdateRefCommand>
        {
            private const string spUpdateRef = "UpdateRef";

            public UpdateRefCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(UpdateRefCommand command, CancellationToken token = default)
            {
                var @ref = new Ref
                {
                    Id = command.Id,
                    FolderId = command.FolderId,
                    Title = command.Title,
                    Url = command.Url,
                    Image = command.Image,
                    Priority = command.Priority,
                    Note = command.Note,
                    Tags = command.Tags
                        ?.Select(t => new Tag { Name = t })
                        ?.ToList() 
                        ?? new List<Tag>()
                };

                await dapperContext.ExecuteProcedureAsync(spUpdateRef, new { @ref = @ref.ToUdt(), tags = @ref.Tags.ToUdt() }, token);

                return Result.Success();
            }
        }
    }
}
