using System.ComponentModel.DataAnnotations;

namespace AuthExample.Models
{
    public class RegisterUserRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
