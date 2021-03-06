
namespace ReadItLater.Web.Server.Utils
{
    public class AuthenticationConfiguration
    {
        public static string AuthenticationSection = "Authentication";

        public string? Secret { get; set; }
        public int JwtTokenExpirationTimeInMinutes { get; set; } = 2;
        public int RefreshTokenExpirationTimeInHours { get; set; } = 168;
    }
}
