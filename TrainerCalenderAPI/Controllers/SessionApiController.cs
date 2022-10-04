using Microsoft.AspNetCore.Mvc;
using TrainerCalenderAPI.Models.Dto;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Controllers
{
    [Route("api/sessions")]
    public class SessionApiController : Controller
    {
        protected ResponseDto _responseDto;

        //interface Variable
        private ISessionRepository _sessionRepository;

        //Constructor 
        public SessionApiController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            this._responseDto = new ResponseDto();
        }

        //method to retrive All Sessions...
        //Status: Done
        [HttpGet]
        [Route("GetAllSessions")]
        public async Task<object> Get()
        {
            try
            {
                
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetAllSessionsDtos();
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any()) 
                { 
                    _responseDto.DisplayMessage = "successfully Get All Sessions...";
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status : Done
        //Get All Sessions by Start and End Date
        [HttpGet]
        [Route("GetSessionsByDateRange/{startDate}/{endDate}")]
        public async Task<object> GetSessionByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
               
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetSessionsByDateRange(startDate, endDate);
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any())
                { 
                    _responseDto.DisplayMessage = "Successfully Get All Sessions...";
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status : Done
        //Get All Sessions of a Trainer by start and end date...
        [HttpGet]
        [Route("GetSessionsByTrainerDateRange/{trainerId}/{startDate}/{endDate}")]
        public async Task<object> GetSessionByTrainerDateRange(string trainerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetSessionsByTrainerDateRange(trainerId, startDate, endDate);
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any())
                { 
                    _responseDto.DisplayMessage = "Successfully Get All Sessions...";
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status : Done
        //get All Sessions of a trainer for a Date...
        [HttpGet]
        [Route("GetSessionsByTrainerDate/{trainerId}/{seletedDate}")]
        public async Task<object> GetSessionByTrainerDate(string trainerId, DateTime seletedDate)
        {
            try
            {
                
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetAllSessionsByTrainerForDate(trainerId, seletedDate);
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any())
                {
                    _responseDto.DisplayMessage = "Successfully Get All Sessions...";
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //


        //Status:Done
        //method : Get All Sessions of a trainer
        [HttpGet]
        [Route("GetSessionByTrainer/{trainerId}")]
        public async Task<object> GetSessionByTrainer(string trainerId)
        {
            try
            {   
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetSessionByTrainerId(trainerId);
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any())
                {                   
                    _responseDto.DisplayMessage = "successfully Get Session By Trainer Id: " + trainerId;
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
                
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status:Done
        //method : Get All Session of a Course...
        [HttpGet]
        [Route("GetSessionByCourse/{courseId}")]
        public async Task<object> GetSessionByCourse(int courseId)
        {
            try
            {   
                IEnumerable<SessionDto> sessionDtos = await _sessionRepository.GetSessionByCourseId(courseId);
                _responseDto.Result = sessionDtos;
                if (sessionDtos.Any())
                {
                    _responseDto.DisplayMessage = "successfully Get Session By courseId Id: " + courseId;
                }
                else
                {
                    _responseDto.DisplayMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status:Done
        //method :  Add a session...
        [HttpPost]
        [Route("CreateSession")]
        public async Task<object> Post([FromBody] SessionCreateDto sessionDto)
        {
            try
            {
                SessionCreateDto proDto = await _sessionRepository.CreateSession(sessionDto);
                _responseDto.Result = proDto;
                _responseDto.DisplayMessage = "successfully Created Session...";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status:Done
        //method : Delete a Session...
        [HttpDelete]
        [Route("DeleteSession/{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool res = await _sessionRepository.DeleteSession(id);
                _responseDto.Result = res;
                if (res)
                {
                    _responseDto.DisplayMessage = "successfully Deleted Session";
                }
                else
                {
                    _responseDto.DisplayMessage = "Session Not Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status:Done
        //method : Delete All of a Trainer...
        [HttpDelete]
        [Route("DeleteSessionByTrainerId/{id}")]
        public async Task<object> DeleteByTrainerId(string id)
        {
            try
            {
                bool res = await _sessionRepository.DeleteSessionByTrainerId(id);
                _responseDto.Result = res;
                if (res)
                {
                    _responseDto.DisplayMessage = "successfully Deleted Session";
                }
                else
                {
                    _responseDto.DisplayMessage = "Session Not Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status:Done
        //method : Delete All of a Trainer...
        [HttpDelete]
        [Route("DeleteSessionByDateRange/{startDate}/{endDate}")]
        public async Task<object> DeleteSessionByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                bool res = await _sessionRepository.DeleteSessionByDateRange(startDate, endDate);
                _responseDto.Result = res;
                if (res)
                {
                    _responseDto.DisplayMessage = "successfully Deleted Session";
                }
                else
                {
                    _responseDto.DisplayMessage = "Session Not Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
        //

        //Status: Done
        //method : Update Session...
        [HttpPut]
        [Route("UpdateSession")]
        public async Task<object> Put([FromBody] SessionCreateDto sessionDto)
        {
            try
            {
                SessionCreateDto proDto = await _sessionRepository.CreateSession(sessionDto);
                _responseDto.Result = proDto;
                _responseDto.DisplayMessage = "successfully Updated Session";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }

    }
}
