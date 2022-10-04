using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainerCalenderAPI.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string TrainingMode { get; set; }

        public string TrainingLocation { get; set; }

        public string TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer trainer { get; set; }

        public int SkillId { get; set; }
        [ForeignKey("Id")]
        public virtual ICollection<Skill> Skills { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("Id")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
