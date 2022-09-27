using TrainerCalenderAPI.Models;

namespace TrainerCalenderAPI.Repository.IRepository
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<object>> GetAllTrainers();
        Task<Trainer> GetTrainersById(string id);
        Task<object> GetTrainersBySkill(int id);
       
        Task<Trainer> AddTrainer(Trainer trainer);
        Task<Trainer> UpdateTrainer(Trainer trainer);
        Task<Trainer> DeleteTrainer(string id);
    }
}
