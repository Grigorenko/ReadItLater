
namespace ReadItLater.Web.Client.Services.Models
{
    public class Badge
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsArrowUpActive { get; set; }
        public bool IsArrowDownActive { get; set; }

        public int Position { get; set; }
    }
}
