using Microsoft.EntityFrameworkCore;
using TrainerCalenderAPI.DbContexts;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;
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

        public async Task<object> DeleteTrainer(string id)
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
            //var trainerId = await _applicationDbcontext.Trainers.ToListAsync();
            //var trainerDetails = new List<Trainer>();
            //for (int i = 0; i < trainerId.Count; i++)
            //{
            //    trainerDetails = _applicationDbcontext.Trainers
            //                        .Include(p => p.User)
            //                        .Include(p => p.Skills)
            //                        .ToList();

            //}
            //return trainerDetails;
            List<Trainer> trainerList = await _applicationDbcontext.Trainers.Include(s => s.Skills).ToListAsync();

            List<TrainerModelDto> trainers = new List<TrainerModelDto>();
            foreach (var trainer in trainerList)
            {
                List<SkillModelDto> skills = new List<SkillModelDto>();
                foreach (var skill in trainer.Skills)
                {
                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    };
                    skills.Add(skillVM);
                }

                var tr = new TrainerModelDto()
                {
                    EmpId = trainer.Id,
                    Name = _applicationDbcontext.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                    Skills = skills
                };
                trainers.Add(tr);
            }

            return trainers;
        }

        public async Task<object> GetTrainersById(string id)
        {
            //return await _applicationDbcontext.Trainers
            //                        .Include(p => p.User)
            //                        .Include(p => p.Skills)
            //                        .FirstOrDefaultAsync(a => a.Id == id);
            var trainer = await _applicationDbcontext.Trainers.Include(x => x.Skills).FirstOrDefaultAsync();
            List<SkillModelDto> skills = new List<SkillModelDto>();
            foreach (var skill in trainer.Skills)
            {
                var skillVM = new SkillModelDto
                {
                    Id = skill.Id,
                    Name = skill.Name
                };
                skills.Add(skillVM);
            }
            var tr = new TrainerModelDto()
            {
                EmpId = trainer.Id,
                Name = _applicationDbcontext.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                Skills = skills
            };
            return tr;
        }

        //public async Task<object> AddTrainer(TrainerViewModelDto trainer)
        //{

        //    await _applicationDbcontext.Trainers.AddAsync(trainer);
        //    await _applicationDbcontext.Users.AddAsync(trainer.User);
            
        //    await _applicationDbcontext.SaveChangesAsync();

        //    return trainer;
        //}

        public async Task<object> UpdateTrainer(TrainerModelDto trainer, string id)
        {

            var result = await _applicationDbcontext.Trainers
                                           .Include(x => x.User)
                                           .FirstOrDefaultAsync(a => a.Id == id);

            if (result != null)
            {
                // We can update more fields using below code.
                result.User.Name = Convert.ToString(trainer.Name);
                result.User.Email = Convert.ToString(trainer.Email);
                result.User.PhoneNumber = Convert.ToString(trainer.PhoneNum);
                _applicationDbcontext.Users.Update(result.User);
                //_applicationDbcontext.Users.Update(result.User);
                _applicationDbcontext.SaveChanges();
                await _applicationDbcontext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        //We have used Skill table ID here to retrive a data
        public async Task<object> GetTrainersBySkill(int id)
        {
            //return await _applicationDbcontext.Skills
            //                                  .Include(x => x.Trainers)
            //                                     .ThenInclude(x => x.User)
            //                                  .FirstOrDefaultAsync(a => a.Id == id);
            var skill = await _applicationDbcontext.Skills
                                              .Include(x => x.Trainers)
                                                 .ThenInclude(x => x.User)
                                              .FirstOrDefaultAsync(a => a.Id == id);
            List<TrainerModelDto> trainers = new List<TrainerModelDto>();
            foreach (var trainer in skill.Trainers)
            {
                var tr = new TrainerModelDto()
                {
                    EmpId = trainer.Id,
                    Name = _applicationDbcontext.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                    Email = _applicationDbcontext.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                    PhoneNum = _applicationDbcontext.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                };
                trainers.Add(tr);
            }
            return trainers;
        }

    }
}
