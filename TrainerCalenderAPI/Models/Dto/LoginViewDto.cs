using System.ComponentModel.DataAnnotations;

namespace TrainerCalenderAPI.Models.Dto
{
    public class LoginViewDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
