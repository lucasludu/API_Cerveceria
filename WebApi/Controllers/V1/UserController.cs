using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Application.Parameters;
using Microsoft.AspNetCore.Authorization;
using Application.Features._users.Queries;
using Application.Features._users.Commands;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class UserController : BaseApiController
    {
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] RequestParameters filter)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery { Parameters = filter }));
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPatch("toggle-active/{id}")]
        public async Task<IActionResult> ToggleUserActive(string id)
        {
            return Ok(await Mediator.Send(new ToggleUserActiveCommand { Id = id }));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand { Id = id }));
        }
    }
}
