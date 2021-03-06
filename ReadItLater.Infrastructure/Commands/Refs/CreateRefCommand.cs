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
    public class CreateRefCommand : ICommand
    {
        public CreateRefCommand(Guid? id, Guid? folderId, string title, string url, string? image, Priority priority, string? note, string[] tags)
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

        public Guid? Id { get; set; }
        public Guid? FolderId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Image { get; set; }
        public Priority Priority { get; set; }
        public string? Note { get; set; }
        public string[] Tags { get; set; }

        [AuditLogCommand]
        [DataValidationCommand]
        public class CreateRefCommandHandler : AsyncCommandHandlerBase<CreateRefCommand>
        {
            private const string spCreateRef = "CreateRef";

            public CreateRefCommandHandler(
                IDapperContext dapperContext,
                IUserProvider userProvider)
                : base(dapperContext, userProvider)
            {
            }

            public override async Task<Result> HandleAsync(CreateRefCommand command, CancellationToken token = default)
            {
                var @ref = new Ref
                {
                    Id = command.Id ?? Guid.NewGuid(),
                    FolderId = command.FolderId,
                    Title = command.Title,
                    Url = command.Url,
                    Image = command.Image,
                    Priority = command.Priority,
                    Note = command.Note,
                    Date = DateTime.UtcNow,
                    Tags = command.Tags
                        ?.Select(t => new Tag { Name = t })
                        ?.ToList() 
                        ?? new List<Tag>()
                };

                await dapperContext.ExecuteProcedureAsync(spCreateRef, new { @ref = @ref.ToUdt(), tags = @ref.Tags.ToUdt() }, token);

                return Result.Success();
            }
        }
    }
}
