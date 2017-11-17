#region usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    public class UserProfile
    {
        [Required]
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        [StringLength(128)]
        public string Avatar { get; set; }

        public virtual User User { get; set; }
    }
}