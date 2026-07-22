using Application.Features.Wholesalers.Commands;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class WholesalerController : BaseApiController
    {
        [HttpPost("{id}/sale")]
        public async Task<IActionResult> AddSale(Guid id, [FromBody] AddBeerSaleCommand command)
        {
            if (id != command.WholesalerId)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }
    }
}
