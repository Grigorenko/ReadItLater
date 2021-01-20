using ReadItLater.Web.Client.Services.Models;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface ISortingChanged : IContext
    {
        Task Handle(Guid? folderId, Guid? tagId, Badge[] badges);
    }
}
