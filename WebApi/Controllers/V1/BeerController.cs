using Application.Features.Beers.Commands;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BeerController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateBeerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateBeerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteBeerCommand { Id = id }));
        }
    }
}
