using Application.Features.Wholesalers.Commands;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class QuoteController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> RequestQuote(RequestQuoteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
