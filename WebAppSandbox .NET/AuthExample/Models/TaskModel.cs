using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthExample.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "TaskName is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "TaskDescription is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string TaskDescription { get; set; }

        [Required(ErrorMessage = "TaskStatus is required")]
        [Column(TypeName = "bit")]
        public bool TaskStatus { get; set; }

        public string UserId { get; set; } // TODO: Determine if this is the right way to associate an ID... Currently using string (guid) to relate to IdentityUser
    }
}