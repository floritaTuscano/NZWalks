using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
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

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
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


    }
}
