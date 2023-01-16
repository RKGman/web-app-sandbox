using System.ComponentModel.DataAnnotations;

namespace AuthExample.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
