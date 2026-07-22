using Application.Features.Beers.Queries;
using Application.Parameters;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BreweryController : BaseApiController
    {
        [HttpGet("{id}/beers")]
        public async Task<IActionResult> GetBeers(Guid id, [FromQuery] RequestParameters filter)
        {
            var query = new GetBeersByBreweryIdQuery 
            { 
                BreweryId = id, 
                PageNumber = filter.PageNumber, 
                PageSize = filter.PageSize 
            };
            return Ok(await Mediator.Send(query));
        }
    }
}
