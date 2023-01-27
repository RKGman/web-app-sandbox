using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthExample.Models
{
    public class ProfileModel
    {
        [Key]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        [Column(TypeName = "nvarchar(1000)")] // TODO: Does this mean this is required? Apparently this can't be blank...
        public string AboutMe { get; set; }

        public string UserId { get; set; } // TODO: Determine if this is the right way to associate an ID... Currently using string (guid) to relate to IdentityUser
    }
}