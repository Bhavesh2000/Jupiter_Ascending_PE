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



// Create Methode

        // Request Api : https://localhost:7026/api/sessions/CreateSession
        //Method to insert session Data to Database..
        //Status : Pending
        public async Task<SessionCreateDto> CreateSession(SessionCreateDto session)
        {
            Session ses = _mapper.Map<SessionCreateDto, Session>(session);
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
            return _mapper.Map<Session, SessionCreateDto>(ses);
        }

// Delete Methode

        // Request Api : 
        //method to delete session by sessionId
        //Status : Done
        public async Task<bool> DeleteSession(int sessionId)
        {
            try
            {
                Session session = await _db.Sessions.Where(s => s.Id == sessionId).FirstOrDefaultAsync();
                if (session == null)
                {
                    return false;
                }
                _db.Sessions.Remove(session);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Request Api :
        //method to delete session by selected Date
        //Status : Done
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
                        _db.Sessions.Remove(session);
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Request Api :
        //method to delete session by Date range(Start Date and end date)
        //Status : Done
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
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Request Api : 
        //Method to delete Session By Trainer Id
        //Status : Done
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
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
 // Get Methods 

        // Request Api : https://localhost:7026/api/sessions/GetSessionsByTrainerDate/1/2022-10-03%2009%3A30%3A32.0000000
        // date Format :- 08-21-2022 09:30:32
        //Method to fetch all session By Trainer Selected Date
        //Status : Done
        public async Task<IEnumerable<SessionDto>> GetAllSessionsByTrainerForDate(string TrainerId, DateTime selectedDate)
        {
            DateTime tempDate = Convert.ToDateTime(selectedDate);
            string tempFormatedDate = string.Format("{0:MM/dd/yyyy}", tempDate);

            List<Session> sessionList = await _db.Sessions
                .Include(s => s.trainer)
                .Include(s => s.Courses)
                .Include(s => s.Skills)
                .ToListAsync();

            List<SessionDto> result = new List<SessionDto>();

            foreach (var session in sessionList)
            {
                DateTime Startdate = Convert.ToDateTime(session.StartDate);
                string FormatedStartDate = string.Format("{0:MM/dd/yyyy}", Startdate);

                if (
                session.TrainerId == TrainerId
                &&
                DateTime.Parse(FormatedStartDate, CultureInfo.InvariantCulture) 
                == 
                DateTime.Parse(tempFormatedDate, CultureInfo.InvariantCulture)
                )
                {

                    var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId).FirstOrDefaultAsync();

                    var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                    var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                    };

                    var courseVm = new CourseDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name
                    };

                    var tr = new TrainerModelDto()
                    {
                        EmpId = _db.Users.First(x => x.Id.Equals(trainer.Id)).Id,
                        Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                        Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                        PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                    };

                    var sessionDto = new SessionDto
                    {
                        Id = session.Id,
                        CourseId = session.CourseId,
                        SkillId = session.SkillId,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        TrainerId = session.TrainerId,
                        TrainingMode = session.TrainingMode,
                        TrainingLocation = session.TrainingLocation,
                        Skill = skillVM,
                        Course = courseVm,
                        Trainer = tr,
                    };
                    result.Add(sessionDto);

                }
            }

            return result;
        }


        // Request Api : https://localhost:7026/api/sessions/GetAllSessions
        //Method To Fetch all Session..
        //Status : Done
        public async Task<IEnumerable<SessionDto>> GetAllSessionsDtos()
        {

            List<Session> sessionList = await _db.Sessions
                .Include(s => s.trainer)
                .Include(s => s.Courses)
                .Include(s => s.Skills)
                .ToListAsync();

            
            List<SessionDto> sessions = new List<SessionDto>();

            foreach (var session in sessionList)
            {
                var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId ).FirstOrDefaultAsync();

                var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                var skillVM = new SkillModelDto
                {
                    Id = skill.Id,
                    Name = skill.Name,
                };

                var courseVm = new CourseDto
                {
                    CourseId = course.Id,
                    CourseName = course.Name
                };

                var tr = new TrainerModelDto()
                {
                    EmpId = trainer.Id,
                    Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                    Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                    PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                };

                var sessionDto = new SessionDto
                {
                    Id = session.Id,
                    CourseId = session.CourseId,
                    SkillId = session.SkillId,
                    StartDate = session.StartDate,
                    EndDate = session.EndDate,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    TrainerId = session.TrainerId,
                    TrainingMode = session.TrainingMode,
                    TrainingLocation = session.TrainingLocation,
                    Skill = skillVM,
                    Course = courseVm,
                    Trainer = tr,
                };
                sessions.Add(sessionDto);

            }
            return sessions;

        }

        // Request Api : https://localhost:7026/api/sessions/GetSessionByCourse/1
        //Method to Fetch All Session By Selected Course
        // Status : Done
        public async Task<IEnumerable<SessionDto>> GetSessionByCourseId(int courseId)
        {

            List<Session> sessionList = await _db.Sessions
               .Include(s => s.trainer)
               .Include(s => s.Courses)
               .Include(s => s.Skills)
               .ToListAsync();

            List<SessionDto> result = new List<SessionDto>();

            foreach (var session in sessionList)
            {
                if(session.CourseId == courseId)
                { 
                    var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId).FirstOrDefaultAsync();

                    var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                    var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                    };

                    var courseVm = new CourseDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name
                    };

                    var tr = new TrainerModelDto()
                    {
                        EmpId = trainer.Id,
                        Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                        Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                        PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                    };

                    var sessionDto = new SessionDto
                    {
                        Id = session.Id,
                        CourseId = session.CourseId,
                        SkillId = session.SkillId,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        TrainerId = session.TrainerId,
                        TrainingMode = session.TrainingMode,
                        TrainingLocation = session.TrainingLocation,
                        Skill = skillVM,
                        Course = courseVm,
                        Trainer = tr,
                    };
                    result.Add(sessionDto);

                }
            }

            return result;

        }


        // Request Api : https://localhost:7026/api/sessions/GetSessionByTrainer/1
        //Method to Fetch All Session By Trainer 
        //Status : Done
        public async Task<IEnumerable<SessionDto>> GetSessionByTrainerId(string trainerId)
        {
            List<Session> sessionList = await _db.Sessions
               .Include(s => s.trainer)
               .Include(s => s.Courses)
               .Include(s => s.Skills)
               .ToListAsync();

            List<SessionDto> result = new List<SessionDto>();

            foreach (var session in sessionList)
            {
                if (session.TrainerId == trainerId)
                {
                    var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId).FirstOrDefaultAsync();

                    var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                    var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                    };

                    var courseVm = new CourseDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name
                    };

                    var tr = new TrainerModelDto()
                    {
                        EmpId = trainer.Id,
                        Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                        Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                        PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                    };

                    var sessionDto = new SessionDto
                    {
                        Id = session.Id,
                        CourseId = session.CourseId,
                        SkillId = session.SkillId,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        TrainerId = session.TrainerId,
                        TrainingMode = session.TrainingMode,
                        TrainingLocation = session.TrainingLocation,
                        Skill = skillVM,
                        Course = courseVm,
                        Trainer = tr,
                    };
                    result.Add(sessionDto);

                }
            }

            return result;
        }


        // Request Api : https://localhost:7026/api/sessions/GetSessionsByDateRange/2022-10-03%2009%3A30%3A32.0000000/2022-10-03%2009%3A30%3A32.0000000
        // date Format :- 08-21-2022 09:30:32
        //Fetch All Session By Date Range(Start Date And End Date)
        //Status : Done
        public async Task<IEnumerable<SessionDto>> GetSessionsByDateRange(DateTime PassedstartDate, DateTime PassedendDate)
        {
            DateTime tempStartDate = Convert.ToDateTime(PassedstartDate);
            string tempFormatedStartDate = string.Format("{0:MM/dd/yyyy}", tempStartDate);

            DateTime tempEndDate = Convert.ToDateTime(PassedendDate);
            string tempFormatedEndDate = string.Format("{0:MM/dd/yyyy}", tempEndDate);

            List<Session> sessionList = await _db.Sessions
                .Include(s => s.trainer)
                .Include(s => s.Courses)
                .Include(s => s.Skills)
                .ToListAsync();

            List<SessionDto> result = new List<SessionDto>();

            foreach (var session in sessionList)
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
                    var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId).FirstOrDefaultAsync();

                    var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                    var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                    };

                    var courseVm = new CourseDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name
                    };

                    var tr = new TrainerModelDto()
                    {
                        EmpId = trainer.Id,
                        Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                        Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                        PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                    };

                    var sessionDto = new SessionDto
                    {
                        Id = session.Id,
                        CourseId = session.CourseId,
                        SkillId = session.SkillId,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        TrainerId = session.TrainerId,
                        TrainingMode = session.TrainingMode,
                        TrainingLocation = session.TrainingLocation,
                        Skill = skillVM,
                        Course = courseVm,
                        Trainer = tr,
                    };
                    result.Add(sessionDto);
                }
            }

            return _mapper.Map<List<SessionDto>>(result);
        }


        // Request Api : https://localhost:7026/api/sessions/GetSessionsByTrainerDateRange/1/2022-10-03%2009%3A30%3A32.0000000/2022-10-03%2009%3A30%3A32.0000000 
        // date Format :- 08-21-2022 09:30:32
        //Fetch All Session of Perticular Trainer By Selected Date Range
        //Status : Done
        public async Task<IEnumerable<SessionDto>> GetSessionsByTrainerDateRange(string TrainerId, DateTime startDate, DateTime endDate)
        {
            DateTime tempStartDate = Convert.ToDateTime(startDate);
            string tempFormatedStartDate = string.Format("{0:MM/dd/yyyy}", tempStartDate);

            DateTime tempEndDate = Convert.ToDateTime(endDate);
            string tempFormatedEndDate = string.Format("{0:MM/dd/yyyy}", tempEndDate);

            List<Session> sessionList = await _db.Sessions
                .Include(s => s.trainer)
                .Include(s => s.Courses)
                .Include(s => s.Skills)
                .ToListAsync();

            List<SessionDto> result = new List<SessionDto>();

            foreach (var session in sessionList)
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
                    var trainer = await _db.Trainers.Where(x => x.Id == session.TrainerId).FirstOrDefaultAsync();

                    var course = await _db.Courses.Where(x => x.Id == session.CourseId).FirstOrDefaultAsync();

                    var skill = await _db.Skills.Where(x => x.Id == session.SkillId).FirstOrDefaultAsync();

                    var skillVM = new SkillModelDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                    };

                    var courseVm = new CourseDto
                    {
                        CourseId = course.Id,
                        CourseName = course.Name
                    };

                    var tr = new TrainerModelDto()
                    {
                        EmpId = trainer.Id,
                        Name = _db.Users.First(x => x.Id.Equals(trainer.Id)).Name,
                        Email = _db.Users.First(x => x.Id.Equals(trainer.Id)).Email,
                        PhoneNum = _db.Users.First(x => x.Id.Equals(trainer.Id)).PhoneNumber
                    };

                    var sessionDto = new SessionDto
                    {
                        Id = session.Id,
                        CourseId = session.CourseId,
                        SkillId = session.SkillId,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        TrainerId = session.TrainerId,
                        TrainingMode = session.TrainingMode,
                        TrainingLocation = session.TrainingLocation,
                        Skill = skillVM,
                        Course = courseVm,
                        Trainer = tr,
                    };
                    result.Add(sessionDto);
                }
            }

            return _mapper.Map<List<SessionDto>>(result);
        }
    }
}
