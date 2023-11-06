using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        /* public IActionResult GetAll()
         {
             // get data from database
             var regions = dbContext.Regions.ToList();
             // map the domain model to the DTO
             var regionsDTO = new List<RegionDto>();
             foreach (var region in regions)
             {
                 regionsDTO.Add(new RegionDto()
                 {
                     Id = region.Id,
                     Code = region.Code,
                     Name = region.Name,
                     regionImageUrl = region.regionImageUrl
                 });
             }
             //return the DTO back to the client
             return Ok(regionsDTO);
         }*/
        [HttpGet]
        //asynchronous
        public async Task<IActionResult> GetAll()
        {
            // get data from database

            var regions = await regionRepository.GetAllAsync();

            // map the domain model to the DTO
            var regionsDTO = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDTO.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    regionImageUrl = region.regionImageUrl
                });
            }
            //return the DTO back to the client
            return Ok(regionsDTO);
        }
        // GET SINGLE REGION ( GET REGION BY ID)
        // https://localhost:7063/api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        /* public IActionResult GetById([FromRoute] Guid id)
         {
             //var region = dbContext.Regions.Find(id);
             var region = dbContext.Regions.FirstOrDefault(r => r.Id == id);
             var regionsDTO = new List<RegionDto>();
             if (region == null)
             {
                 return NotFound();
             }
             regionsDTO.Add(new RegionDto()
             {
                 Id = region.Id,
                 Code = region.Code,
                 Name = region.Name,
                 regionImageUrl = region.regionImageUrl
             });
             return Ok(regionsDTO);
         }*/
        // asynchronouse
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            var region = await regionRepository.GetByIdAsync(id);
            var regionsDTO = new List<RegionDto>();
            if (region == null)
            {
                return NotFound();
            }
            regionsDTO.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                regionImageUrl = region.regionImageUrl
            });
            return Ok(regionsDTO);
        }
        // POST TO CREATE NEW REGION
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert the DTO to domain model

            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                regionImageUrl = addRegionRequestDto.regionImageUrl
            };

            // Use the domain model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map domain model back to DTO
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                regionImageUrl = regionDomainModel.regionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDTO);
        }

        // update region
        // PUT, using id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDTO.Code,
                Name = updateRegionRequestDTO.Name,
                regionImageUrl = updateRegionRequestDTO.regionImageUrl
            };
            // check if region exists
            regionDomainModel= await regionRepository.UpdateAsync(id, regionDomainModel);
            // convert domain model to dto
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                regionImageUrl = regionDomainModel.regionImageUrl
            };
            return Ok(regionDTO);
        }
        // Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            return Ok("Region deleted");
        }

    }
}
