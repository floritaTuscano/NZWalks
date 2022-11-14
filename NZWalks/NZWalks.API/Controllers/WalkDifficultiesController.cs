using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;
        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var regions = await walkDifficultyRepository.GetAllAsync();
            return Ok(regions);//regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkDifficultiesAsync")]
        public async Task<IActionResult> GetWalkDifficultiesAsync(Guid id)
        {
            var walkDifficulties = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulties == null)
            {
                return NotFound();
            }
            else
            {
                var walkDifficultiesDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulties);
                return Ok(walkDifficultiesDTO);//regionDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultiesAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //validate //Fluent Validation is added 
            //if(!(ValidateAddWalkDifficultiesAsync(addWalkDifficultyRequest)))
            //{
            //    return BadRequest(ModelState);
            //}
            //request(DTO) to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            //pass detils to repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return CreatedAtAction(nameof(GetWalkDifficultiesAsync),
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkDifficultiesAsync(Guid id)
        {
            //Get region from database
            var walkDiffcultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            //If null not found
            if (walkDiffcultyDomain == null)
            {
                return NotFound();
            }

            //Convert response back to DTO
            var WalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffcultyDomain);
            //return OK response
            return Ok(WalkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkDifficultiesAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //validate // Fluent validation is added
            //if (!(ValidateUUpdateWalkDifficultiesAsync(updateWalkDifficultyRequest)))
            //{
            //    return BadRequest(ModelState);
            //}
            //request(DTO) to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //call repository to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            //If null then NotFound
            if (walkDifficultyDomain == null)
            {
                return NotFound();

            }
            //convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //return OK response
            return Ok(walkDifficultyDTO);
        }
        #region Private Methods

        private  bool ValidateAddWalkDifficultiesAsync(Models.DTO.AddWalkDifficultyRequest AddWalkDifficultyRequest)
        {
            if (AddWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(AddWalkDifficultyRequest), $"can not be empty.");
                return false;
            }
            if (string.IsNullOrEmpty(AddWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(AddWalkDifficultyRequest.Code), $"{nameof(AddWalkDifficultyRequest.Code)} is required.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }
        private bool ValidateUUpdateWalkDifficultiesAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"can not be empty.");
                return false;
            }
            if (string.IsNullOrEmpty(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} is required.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
            #endregion
        }
}
