namespace TrainerCalenderAPI.Models.Dto
{
    public class SessionDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TrainerId { get; set; }

        public string TrainingMode { get; set; }

        public string TrainingLocation { get; set; }
    }
}
