
using System.ComponentModel.DataAnnotations;
namespace TrainerCalenderAPI.Models
{
    public class Course
    {
        public Course()
        {
            this.Skills = new HashSet<Skill>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
