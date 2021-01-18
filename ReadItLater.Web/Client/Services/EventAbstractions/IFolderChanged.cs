using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface IFolderChanged : IContext
    {
        Task Handle(Guid folderId, Guid? tagId);
    }
}
