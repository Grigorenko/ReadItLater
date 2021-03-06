using System.Threading.Tasks;

namespace ReadItLater.HtmlParser
{
    public interface IHtmlParser
    {
        Task<(string? title, string? image)> GetMetaAsync(string url);
    }
}
