using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TrainerCalenderAPI.DbContexts;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Repository
{
    public class SessionRepository : ISessionRepository
    {
        //Database context class object
        private readonly ApplicationDbContext _db;

        private IMapper _mapper;

        //Constructor
        public SessionRepository(ApplicationDbContext applicationDB, IMapper mapper)
        {
            _db = applicationDB;
            _mapper = mapper;
        }

        //Method to check database Session table is empty or not
        public async Task<bool> CheckSessionTableIsEmptyOrNot()
        {
            int count = _db.Sessions.Count();
            if (count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        //Method to insert session Data to Database..
        public async Task<SessionDto> CreateSession(SessionDto session)
        {
            Session ses = _mapper.Map<SessionDto, Session>(session);
            Console.WriteLine("Session" + ses);
            if (session.Id > 0)
            {
                _db.Update<Session>(ses);
            }
            else
            {
                _db.Sessions.Add(ses);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Session, SessionDto>(ses);
        }

        //method to delete session by sessionId
        public async Task<bool> DeleteSession(int sessionId)
        {
            try
            {
                Session session = await _db.Sessions.FirstOrDefaultAsync(u => u.Id == sessionId);
                if (session == null)
                {
                    return false;
                }
                _db.Sessions.Remove(session);
                _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //method to delete session by selected Date
        public async Task<bool> DeleteSessionByDate(DateTime SelectedDate)
        {
            try
            {
                List<Session> sessions = await _db.Sessions.ToListAsync();
                foreach (var session in sessions)
                {
                    if (session == null)
                    {
                        return false;
                    }
                    if (SelectedDate >= session.StartDate && SelectedDate <= session.EndDate)
                    {
                        Console.WriteLine("Session :" + session.Id);
                        //_db.sessions.Remove(session);
                    }
                }
                _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //method to delete session by Date range(Start Date and end date)
        public async Task<bool> DeleteSessionByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {

                DateTime tempStartDate = Convert.ToDateTime(startDate);
                string tempFormatedStartDate = string.Format("{0:MM/dd/yyyy}", tempStartDate);

                DateTime tempEndDate = Convert.ToDateTime(endDate);
                string tempFormatedEndDate = string.Format("{0:MM/dd/yyyy}", tempEndDate);

                List<Session> sessions = await _db.Sessions.ToListAsync();
                foreach (var item in sessions)
                {
                    if (item == null)
                    {
                        return false;
                    }

                    DateTime Startdate = Convert.ToDateTime(item.StartDate);
                    string FormatedStartDate = string.Format("{0:MM/dd/yyyy}", Startdate);

                    DateTime Enddate = Convert.ToDateTime(item.EndDate);
                    string FormatedEndDate = string.Format("{0:MM/dd/yyyy}", Enddate);

                    if (
                        DateTime.Parse(FormatedStartDate, CultureInfo.InvariantCulture) >= DateTime.Parse(tempFormatedStartDate, CultureInfo.InvariantCulture)
                    &&
                        DateTime.Parse(FormatedEndDate, CultureInfo.InvariantCulture) <= DateTime.Parse(tempFormatedEndDate, CultureInfo.InvariantCulture)
                      )
                    {
                        _db.Sessions.Remove(item);
                    }
                }
                _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Method to delete Session By Trainer Id
        public async Task<bool> DeleteSessionByTrainerId(string trainerId)
        {
            try
            {
                List<Session> sessions = await _db.Sessions.Where(x => x.TrainerId == trainerId).ToListAsync();
                if (sessions.Any())
                {
                    foreach (var item in sessions)
                    {
                        if (item == null)
                        {
                            return false;
                        }
                        _db.Sessions.Remove(item);
                    }
                }
                else
                {
                    return false;
                }
                _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Method to fetch all session By Trainer Selected Date
        public async Task<IEnumerable<SessionDto>> GetAllSessionsByTrainerForDate(string TrainerId, DateTime selectedDate)
        {
            DateTime tempDate = Convert.ToDateTime(selectedDate);
            string tempFormatedDate = string.Format("{0:MM/dd/yyyy}", tempDate);

            List<Session> sessions = await _db.Sessions.ToListAsync();
            List<Session> result = new List<Session>();

            foreach (var session in sessions)
            {
                DateTime Startdate = Convert.ToDateTime(session.StartDate);
                string FormatedStartDate = string.Format("{0:MM/dd/yyyy}", Startdate);

                if (
                session.TrainerId == TrainerId
                &&
                DateTime.Parse(FormatedStartDate, CultureInfo.InvariantCulture) == DateTime.Parse(tempFormatedDate, CultureInfo.InvariantCulture)
                )
                {
                    result.Add(session);
                }
            }

            return _mapper.Map<List<SessionDto>>(result);
        }

        //Method To Fetch all Session..
        public async Task<IEnumerable<SessionDto>> GetAllSessionsDtos()
        {
            List<Session> sessions = await _db.Sessions.ToListAsync();
            return _mapper.Map<List<SessionDto>>(sessions);
        }

        //Method to Fetch All Session By Selected Course
        public async Task<IEnumerable<SessionDto>> GetSessionByCourseId(int courseId)
        {
            List<Session> sessions = await _db.Sessions.Where(x => x.CourseId == courseId).ToListAsync();
            return _mapper.Map<List<SessionDto>>(sessions);
        }


        //Method to Fetch All Session By Trainer 
        public async Task<IEnumerable<SessionDto>> GetSessionByTrainerId(string trainerId)
        {
            List<Session> sessions = await _db.Sessions.Where(x => x.TrainerId == trainerId).ToListAsync();
            return _mapper.Map<List<SessionDto>>(sessions);
        }

        // date Format :- 08-21-2022 09:30:32
        //Fetch All Session By Date Range(Start Date And End Date)
        public async Task<IEnumerable<SessionDto>> GetSessionsByDateRange(DateTime PassedstartDate, DateTime PassedendDate)
        {
            DateTime tempStartDate = Convert.ToDateTime(PassedstartDate);
            string tempFormatedStartDate = string.Format("{0:MM/dd/yyyy}", tempStartDate);

            DateTime tempEndDate = Convert.ToDateTime(PassedendDate);
            string tempFormatedEndDate = string.Format("{0:MM/dd/yyyy}", tempEndDate);

            List<Session> sessions = await _db.Sessions.ToListAsync();
            List<Session> result = new List<Session>();

            foreach (var session in sessions)
            {
                DateTime Startdate = Convert.ToDateTime(session.StartDate);
                string FormatedStartDate = string.Format("{0:MM/dd/yyyy}", Startdate);

                DateTime Enddate = Convert.ToDateTime(session.EndDate);
                string FormatedEndDate = string.Format("{0:MM/dd/yyyy}", Enddate);

                if (
                    DateTime.Parse(FormatedStartDate, CultureInfo.InvariantCulture) >= DateTime.Parse(tempFormatedStartDate, CultureInfo.InvariantCulture)
                    &&
                    DateTime.Parse(FormatedEndDate, CultureInfo.InvariantCulture) <= DateTime.Parse(tempFormatedEndDate, CultureInfo.InvariantCulture)
                    )
                {
                    result.Add(session);
                }
            }

            return _mapper.Map<List<SessionDto>>(result);
        }

        //Fetch All Session of Perticular Trainer By Selected Date Range
        public async Task<IEnumerable<SessionDto>> GetSessionsByTrainerDateRange(string TrainerId, DateTime startDate, DateTime endDate)
        {
            DateTime tempStartDate = Convert.ToDateTime(startDate);
            string tempFormatedStartDate = string.Format("{0:MM/dd/yyyy}", tempStartDate);

            DateTime tempEndDate = Convert.ToDateTime(endDate);
            string tempFormatedEndDate = string.Format("{0:MM/dd/yyyy}", tempEndDate);

            List<Session> sessions = await _db.Sessions.ToListAsync();
            List<Session> result = new List<Session>();

            foreach (var session in sessions)
            {
                DateTime Startdate = Convert.ToDateTime(session.StartDate);
                string FormatedStartDate = string.Format("{0:MM/dd/yyyy}", Startdate);

                DateTime Enddate = Convert.ToDateTime(session.EndDate);
                string FormatedEndDate = string.Format("{0:MM/dd/yyyy}", Enddate);

                if (
                session.TrainerId == TrainerId
                &&
                DateTime.Parse(FormatedStartDate, CultureInfo.InvariantCulture) >= DateTime.Parse(tempFormatedStartDate, CultureInfo.InvariantCulture)
                &&
                DateTime.Parse(FormatedEndDate, CultureInfo.InvariantCulture) <= DateTime.Parse(tempFormatedEndDate, CultureInfo.InvariantCulture)
                )
                {
                    result.Add(session);
                }
            }

            return _mapper.Map<List<SessionDto>>(result);
        }
    }
}
