using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadItLater.BL
{
    public interface IHtmlParser
    {
        Task<(string title, string image)> GetMetaAsync(string url);
    }
}
