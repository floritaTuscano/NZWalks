using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Diagnostics;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from datatbasse - Domain walks
            var WalksDomain = await walkRepository.GetAllAsync();

            //convert Domain Wlaks to DTO Walks
            var WalksDTO = mapper.Map<List<Models.DTO.Walk>>(WalksDomain);
            //Return response
            return Ok(WalksDTO);
        }

        [HttpGet]
        [Route("id:guid")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.GetAsync(id);

            //convert Domain object ro DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalksRequest addWalksRequest)
        {
            //validation 
            if (!(await ValidateAddWalksAsync(addWalksRequest)))
            {
                return BadRequest(ModelState);
            }
            //convert DTO to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalksRequest.Length,
                Name = addWalksRequest.Name,
                RegionId = addWalksRequest.RegionId,
                WalkDifficultyId = addWalksRequest.WalkDifficultyId
            };
            //pass domain object to repository to persist this 
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //convert the domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //send DTO response back to client 
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Get region from database
            var walkDomain = await walkRepository.DeleteAsync(id);
            //If null not found
            if (walkDomain == null)
            {
                return NotFound();
            }

            ////Convert response back to DTO
            //var regionDTO = new Models.DTO.Region
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Area = region.Area,
            //    Lat = region.Lat,
            //    Long = region.Long,
            //    Name = region.Name,
            //    Population = region.Population
            //};
            //return OK response
            return Ok(walkDomain);


        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalksAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalksRequest updateWalksRequest) //not working
        {
            //validate
            if (!(await ValidateUpdateWalksAsync(updateWalksRequest)))
            {
                return BadRequest(ModelState);

            }
            //convert DTO to domain object
            var walkDomain = new Models.Domain.Walk
            {

                Length = updateWalksRequest.Length,
                Name = updateWalksRequest.Name,
                RegionId = updateWalksRequest.RegionId,
                WalkDifficultyId = updateWalksRequest.WalkDifficultyId
            };
            //Pass details to repository - Get domain object
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //Handle null(Not Found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            //convert back domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return Ok(walkDTO);

        }

        #region Private Methods
        private async Task<bool> ValidateAddWalksAsync(Models.DTO.AddWalksRequest addWalksRequest)
        {
            if (addWalksRequest == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest), $"can not be empty.");
                return false;
            }
            if (string.IsNullOrEmpty(addWalksRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalksRequest.Name), $"{nameof(addWalksRequest.Name)} is required.");
            }
            if (addWalksRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(addWalksRequest.Length), $"{nameof(addWalksRequest.Length)} should be greater then zero.");
            }
            var region = await regionRepository.GetAsync(addWalksRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest.RegionId), $"{nameof(addWalksRequest.RegionId)} is invalid.");

            }
            var walkdifficulty = await walkDifficultyRepository.GetAsync(addWalksRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest.WalkDifficultyId), $"{nameof(addWalksRequest.WalkDifficultyId)} is invalid.");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateUpdateWalksAsync(Models.DTO.UpdateWalksRequest updateWalksRequest)
        {
            if (updateWalksRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest), $"can not be empty.");
                return false;
            }
            if (string.IsNullOrEmpty(updateWalksRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalksRequest.Name), $"{nameof(updateWalksRequest.Name)} is required.");
            }
            if (updateWalksRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.Length), $"{nameof(updateWalksRequest.Length)} should be greater then zero.");
            }
            var region = await regionRepository.GetAsync(updateWalksRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.RegionId), $"{nameof(updateWalksRequest.RegionId)} is invalid.");

            }
            var walkdifficulty = await walkDifficultyRepository.GetAsync(updateWalksRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalksRequest.WalkDifficultyId), $"{nameof(updateWalksRequest.WalkDifficultyId)} is invalid.");

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
