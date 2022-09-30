namespace TrainerCalenderAPI.Models.Dto
{
    public class TrainerModelDto
    {
        public string EmpId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public List<SkillModelDto> Skills { get; set; }

    }
}
