using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface IRefAdded : IContext
    {
        Task Handle();
    }
}
