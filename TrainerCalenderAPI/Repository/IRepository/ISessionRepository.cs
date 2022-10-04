using TrainerCalenderAPI.Models.Dto;

namespace TrainerCalenderAPI.Repository.IRepository
{
    public interface ISessionRepository
    {
        //1
        Task<IEnumerable<SessionDto>> GetAllSessionsDtos();

        //2
        Task<IEnumerable<SessionDto>> GetSessionsByDateRange(DateTime startDate, DateTime endDate);

        //3
        Task<IEnumerable<SessionDto>> GetSessionsByTrainerDateRange(string TrainerId, DateTime startDate, DateTime endDate);

        //4
        Task<IEnumerable<SessionDto>> GetAllSessionsByTrainerForDate(string TrainerId, DateTime selectedDate);

        //5
        Task<IEnumerable<SessionDto>> GetSessionByTrainerId(string trainerId);

        //6
        Task<IEnumerable<SessionDto>> GetSessionByCourseId(int courseId);

        // One Methode For Create as well as Update Session
        //7,8
        Task<SessionCreateDto> CreateSession(SessionCreateDto session);

        //9
        Task<bool> DeleteSession(int sessionId);

        //10
        Task<bool> DeleteSessionByTrainerId(string trainerId);

        //11
        Task<bool> DeleteSessionByDate(DateTime SelectedDate);

        //11
        Task<bool> DeleteSessionByDateRange(DateTime startDate, DateTime endDate);
    }
}
