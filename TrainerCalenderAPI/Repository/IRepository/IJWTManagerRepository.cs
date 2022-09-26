using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;

namespace TrainerCalenderAPI.Repository.IRepository
{
    public interface IJWTManagerRepository
    {
        Task<Tokens> AuthenticateAsync(LoginViewDto users);
    }
}
