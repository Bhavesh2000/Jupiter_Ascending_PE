using Microsoft.AspNetCore.Identity;

namespace TrainerCalenderAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
