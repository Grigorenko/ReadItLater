using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface ISubMenuClosed : IContext
    {
        Task Handle();
    }
}
