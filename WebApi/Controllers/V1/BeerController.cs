using Application.Features._beers.Commands.CreateBeerCommands;
using Application.Features._beers.Commands.DeleteBeerCommands;
using Application.Features._beers.Commands.UpdateBeerCommands;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Brewery")]
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
            if (id != command.Request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteBeerCommand(id)));
        }
    }
}
