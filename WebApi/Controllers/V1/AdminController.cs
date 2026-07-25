using Application.Features._admin.Queries;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        [HttpGet("beers")]
        public async Task<IActionResult> GetAllBeers()
        {
            return Ok(await Mediator.Send(new GetAllBeersQuery()));
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            return Ok(await Mediator.Send(new GetGlobalStatsQuery()));
        }
    }
}
