using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The First Name field is required")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "The Last Name field is required")]
        public string LastName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "The Phone field is not a valid phone number")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        [Required]
        public string Password { get; set; } = "";
    }
}
