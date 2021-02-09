using System.ComponentModel.DataAnnotations;

namespace ReadItLater.Data.Dtos
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }
    }
}
