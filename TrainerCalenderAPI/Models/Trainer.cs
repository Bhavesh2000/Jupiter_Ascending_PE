using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainerCalenderAPI.Models
{
    public class Trainer
    {
        public Trainer()
        {
            this.Skills = new HashSet<Skill>();
        }

        [Key]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
