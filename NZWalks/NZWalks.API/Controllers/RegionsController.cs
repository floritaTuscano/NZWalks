using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;//inject the service into the class
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            //DTO /Contract Model 
            //without Auto Mapper Code start
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{

            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code=region.Code,
            //        Name=region.Name,
            //        Area=region.Area,
            //        Lat=region.Lat,
            //        Long=region.Long,
            //        Population=region.Population
            //    };

            //    regionsDTO.Add(regionDTO);
            //});
            //without Auto Mapper Code start

            //var regionsDTO = mapper.Map<Models.DTO.Region>(regions);

            return Ok(regions);//regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var regions = await regionRepository.GetAsync(id);
            if (regions == null)
            {
                return NotFound();
            }
            else
            {
                // var regionDTO = mapper.Map<Models.DTO.Region>(regions);
                return Ok(regions);//regionDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //request(DTO) to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            //pass detils to repository
            region = await regionRepository.AddAsync(region);

            //convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from database
            var region = await regionRepository.DeleteAsync(id);
            //If null not found
            if (region == null)
            {
                return NotFound();
            }

            //Convert response back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //return OK response
            return Ok(regionDTO);


        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };
            //update region using repository
            var Updatedregion = await regionRepository.UpdateAsync(id, region);
            //If null then NotFound
            if (Updatedregion == null)
            {
                return NotFound();

            }
            //convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //return OK response
            return Ok(regionDTO);
        }
    }
}
