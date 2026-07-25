using Application.DTOs.Request._wholesaler;
using Application.Features._wholesalers.Commands.AddBeerSaleCommands;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Wholesaler")]
    public class WholesalerController : BaseApiController
    {
        [HttpPost("{id}/sale")]
        public async Task<IActionResult> AddSale(Guid id, [FromBody] AddBeerSaleRequest request)
        {
            if (id != request.WholesalerId)
                return BadRequest("El ID del mayorista en la ruta no coincide con el cuerpo de la solicitud.");

            return Ok(await Mediator.Send(new AddBeerSaleCommand(request)));
        }
    }
}
