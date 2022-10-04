namespace TrainerCalenderAPI.Models.Dto
{
    public class CourseDto
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public ICollection<SkillModelDto> skills { get; set; }
    }
}
