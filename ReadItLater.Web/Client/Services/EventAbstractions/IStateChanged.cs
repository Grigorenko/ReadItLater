using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface IStateChanged : IContext
    {
        Task Handle();
    }
}
