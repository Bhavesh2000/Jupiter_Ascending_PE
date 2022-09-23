using System.ComponentModel.DataAnnotations;
namespace TrainerCalenderAPI.Models
{
    public class Skill
    {
        public Skill()
        {
            this.Trainers = new HashSet<Trainer>();
            this.Courses = new HashSet<Course>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Trainer> Trainers { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
