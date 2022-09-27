using Microsoft.EntityFrameworkCore;
using TrainerCalenderAPI.DbContexts;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Repository
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly ApplicationDbContext _applicationDbcontext;
        public TrainerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbcontext = applicationDbContext;
        }

        public async Task<Trainer> DeleteTrainer(string id)
        {
            var result = await _applicationDbcontext.Trainers.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (result != null)
            {
                _applicationDbcontext.Remove(result); //Will delete from Trainer Table
                
                _applicationDbcontext.Remove(result.User); //Will delete from User Table

                await _applicationDbcontext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<object>> GetAllTrainers()
        {
            //return
            var trainerId = await _applicationDbcontext.Trainers.ToListAsync();
            var trainerDetails = new List<Trainer>();
            for (int i = 0; i < trainerId.Count; i++)
            {
                trainerDetails = _applicationDbcontext.Trainers
                                    .Include(p => p.User)
                                    .Include(p => p.Skills)
                                    .ToList();

            }
            return trainerDetails;
        }

        public async Task<Trainer> GetTrainersById(string id)
        {
            return await _applicationDbcontext.Trainers
                                    .Include(p => p.User)
                                    .Include(p => p.Skills)
                                    .FirstOrDefaultAsync(a => a.Id == id);

        }

        public async Task<Trainer> AddTrainer(Trainer trainer)
        {

            await _applicationDbcontext.Trainers.AddAsync(trainer);
            await _applicationDbcontext.Users.AddAsync(trainer.User);
            
            await _applicationDbcontext.SaveChangesAsync();

            return trainer;
        }

        public async Task<Trainer> UpdateTrainer(Trainer trainer)
        {
            var result = await _applicationDbcontext.Trainers
                                            .Include(x => x.User)
                                            .FirstOrDefaultAsync(a => a.Id == trainer.Id);
            if (result != null)
            {
                // We can update more fields using below code.
                result.User.Name = trainer.User.Name;
                result.User.UserName = trainer.User.UserName;
                result.User.Email = trainer.User.Email;
                result.User.PhoneNumber = trainer.User.PhoneNumber;
                _applicationDbcontext.Trainers.Update(result);
                _applicationDbcontext.Users.Update(result.User);
                _applicationDbcontext.SaveChanges();
                await _applicationDbcontext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        //We have used Skill table ID here to retrive a data
        public async Task<object> GetTrainersBySkill(int id)
        {
            return await _applicationDbcontext.Skills
                                              .Include(x => x.Trainers)
                                                 .ThenInclude(x => x.User)
                                              .FirstOrDefaultAsync(a => a.Id == id);

        }

    }
}
