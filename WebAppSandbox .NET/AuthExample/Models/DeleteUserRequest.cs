using System.ComponentModel.DataAnnotations;

namespace AuthExample.Models
{
    public class DeleteUserRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
