using ReadItLater.Web.Client.Services.Models;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public interface ISortingChosen : IContext
    {
        Task Handle(Badge[] badges);
    }
}
