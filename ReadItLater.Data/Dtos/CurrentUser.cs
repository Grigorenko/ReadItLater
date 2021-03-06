using System.Collections.Generic;

namespace ReadItLater.Data.Dtos
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public Dictionary<string, string>? Claims { get; set; }
        public string? Token { get; set; }

        public static CurrentUser Unauthorized => new CurrentUser { IsAuthenticated = false };
    }
}
