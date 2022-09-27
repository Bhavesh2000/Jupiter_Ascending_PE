using Microsoft.AspNetCore.Mvc;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TrainerController : Controller
    {
        private readonly ITrainerRepository _trainerRepository;
        public TrainerController(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        [HttpGet]
        [Route("/GetAllTrainers")]
        public async Task<ActionResult> GetTrainers()
        {
            try
            {
                return Ok(await _trainerRepository.GetAllTrainers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
            }
        }

        [HttpGet("/GetTrainerById/{id}")]

        public async Task<ActionResult<Trainer>> GetTrainerById(string id)
        {
            try
            {
                var result = await _trainerRepository.GetTrainersById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
            }
        }


        [HttpGet("/GetTrainerBySkill/{id:int}")]
        public async Task<ActionResult<object>> GetTrainerSkill(int id)
        {
            try
            {
                var result = await _trainerRepository.GetTrainersBySkill(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");
            }
        }


        [HttpDelete("/DeleteTrainer/{id}")]
        public async Task<ActionResult<Trainer>> DeleteTrainerById(string id)
        {
            try
            {
                var trainerDelete = await _trainerRepository.GetTrainersById(id);
                if (trainerDelete == null)
                {
                    return NotFound($"Employee Id={id}not Found");
                }
                return await _trainerRepository.DeleteTrainer(id);

            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving Data from Database");

            }
        }

        [HttpPost("/AddTrainer")]
        public async Task<ActionResult<Trainer>> AddNewTrainer([FromBody] Trainer trainer)
        {
            try
            {
                if (trainer == null)
                    return BadRequest("Null Data");


                var createdTrainer = await _trainerRepository.AddTrainer(trainer);

                return CreatedAtAction(nameof(GetTrainerById),
                    new { id = createdTrainer.Id }, createdTrainer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }


        [HttpPut("/UpdateTrainer/{id}")]
        public async Task<ActionResult<Trainer>> UpdateTrainer(string id, Trainer trainer)
        {
            try
            {

                var trainerToUpdate = await _trainerRepository.GetTrainersById(id);

                if (trainerToUpdate == null)
                    return NotFound($"Employee Id={id} not Found");

                return await _trainerRepository.UpdateTrainer(trainer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }



    }

}