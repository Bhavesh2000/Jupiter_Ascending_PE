using Microsoft.AspNetCore.Mvc;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TrainerController : Controller
    {
        private readonly ITrainerRepository _trainerRepository;
        ResponseDto _response;
        public TrainerController(ITrainerRepository trainerRepository)
        {
            this._response = new ResponseDto();
            _trainerRepository = trainerRepository;
        }

        [HttpGet]
        [Route("/GetAllTrainers")]
        public async Task<object> GetTrainers()
        {
            try
            {
                var result = await _trainerRepository.GetAllTrainers();
                _response.Result = Ok(result);
                
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("/GetTrainerById/{id}")]

        public async Task<ActionResult<object>> GetTrainerById(string id)
        {
            try
            {
                var result = await _trainerRepository.GetTrainersById(id);
                if (result == null)
                {
                    _response.Result = NotFound();
                }
                _response.Result =Ok(result);
            }
            catch (Exception ex)
            {
               // return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("/GetTrainerBySkill/{id:int}")]
        public async Task<ActionResult<object>> GetTrainerSkill(int id)
        {
            try
            {
                var result = await _trainerRepository.GetTrainersBySkill(id);
                if (result == null)
                {
                    _response.Result =  NotFound();
                }
                _response.Result = Ok(result);
            }
            catch (Exception ex)
            {
               // return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpDelete("/DeleteTrainer/{id}")]
        public async Task<ActionResult<object>> DeleteTrainerById(string id)
        {
            try
            {
                var trainerDelete = await _trainerRepository.GetTrainersById(id);
                if (trainerDelete == null)
                {
                    _response.DisplayMessage = $"Employee Id={id}not Found";
                }
                var result =  _trainerRepository.DeleteTrainer(id);
                _response.Result = Ok(result);
            }

            catch (Exception ex)
            {
                // return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //[HttpPost("/AddTrainer")]
        //public async Task<ActionResult<object>> AddNewTrainer([FromBody] TrainerViewModel trainer)
        //{
        //    try
        //    {
        //        if (trainer == null)
        //            _response.Result = BadRequest("Null Data");


        //        var createdTrainer = await _trainerRepository.AddTrainer(trainer);

        //        var result = CreatedAtAction(nameof(GetTrainerById),
        //            new { id = createdTrainer.Id }, createdTrainer);
        //        _response.Result = Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //       // return StatusCode(StatusCodes.Status500InternalServerError,
        //          //  "Error creating new employee record");
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}


        [HttpPut("/UpdateTrainer/{id}")]
        public async Task<ActionResult<object>> UpdateTrainer(string id, TrainerModelDto trainer)
        {
            try
            {
                
                var trainerToUpdate = await _trainerRepository.GetTrainersById(id);

                if (trainerToUpdate == null)
                    _response.DisplayMessage =  ($"Employee Id={id} not Found");

                var result = await _trainerRepository.UpdateTrainer(trainer, id);
                _response.Result = Ok(result);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError,
                //    "Error updating data");
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



    }

}