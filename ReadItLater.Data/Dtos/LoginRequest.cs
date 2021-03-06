using System.ComponentModel.DataAnnotations;

namespace ReadItLater.Data.Dtos
{
    public class LoginRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string? UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
