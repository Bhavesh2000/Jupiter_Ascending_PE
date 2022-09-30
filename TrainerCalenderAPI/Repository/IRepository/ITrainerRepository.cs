using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;

namespace TrainerCalenderAPI.Repository.IRepository
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<object>> GetAllTrainers();
        Task<object> GetTrainersById(string id);
        Task<object> GetTrainersBySkill(int id);
       
       // Task<object> AddTrainer(TrainerViewModel trainer);
        Task<object> UpdateTrainer(TrainerModelDto trainer, string id);
        Task<object> DeleteTrainer(string id);
    }
}
