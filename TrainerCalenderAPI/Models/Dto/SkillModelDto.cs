namespace TrainerCalenderAPI.Models.Dto
{
    public class SkillModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator SkillModelDto(CourseDto v)
        {
            throw new NotImplementedException();
        }
    }
}
