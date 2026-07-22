using Application.Features._auth.Commands.LoginCommands;
using Application.Features._auth.Commands.RegisterUserCommands;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Application.Features._auth.DTOs.Request;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await Mediator.Send(new LoginCommand(request));

            return (!result.Succeeded)
                ? Unauthorized(result)
                : Ok(result);
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await Mediator.Send(new RegisterUserCommand(request));

            return (!result.Succeeded)
                ? BadRequest(result)
                : Ok(result);
        }
    }
}
