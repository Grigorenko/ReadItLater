using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface ITagChanged : IContext
    {
        Task Handle(Guid folderId, Guid? tagId);
    }
}
