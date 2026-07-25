using Application.DTOs.Request._wholesaler;
using Application.Features._wholesalers.Commands.RequestQuoteCommands;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Wholesaler,Client")]
    public class QuoteController : BaseApiController
    {
        [HttpPost("request-quote")]
        public async Task<IActionResult> RequestQuote([FromBody] RequestQuoteRequest request)
        {
            return Ok(await Mediator.Send(new RequestQuoteCommand(request)));
        }
    }
}
