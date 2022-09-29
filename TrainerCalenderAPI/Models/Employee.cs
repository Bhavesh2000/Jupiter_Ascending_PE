using System.ComponentModel.DataAnnotations;

namespace TrainerCalenderAPI.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string ContactNumber { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
