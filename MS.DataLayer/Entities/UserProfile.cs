using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.DataLayer.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id       { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        public string Nickname { get; set; }

        [StringLength(60)]
        public string Avatar   { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
