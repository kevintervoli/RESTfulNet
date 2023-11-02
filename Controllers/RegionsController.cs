using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
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

            var regions = await dbContext.Regions.ToListAsync();
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

            var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
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
            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

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
            // check if region exists
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if(regionDomainModel  == null)
            {
                return NotFound();
            }
            // map dto to domain model
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.regionImageUrl = updateRegionRequestDTO.regionImageUrl;

            await dbContext.SaveChangesAsync();
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
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            return Ok("Region deleted");
        }

    }
}
