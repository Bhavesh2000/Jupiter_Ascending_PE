using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainerCalenderAPI.Models;

namespace TrainerCalenderAPI.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Session> Sessions { get; set; }
    }
}
