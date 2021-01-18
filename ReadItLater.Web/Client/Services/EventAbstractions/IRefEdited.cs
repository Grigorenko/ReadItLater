using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface IRefEdited : IContext
    {
        Task Handle(Guid id);
    }
}
