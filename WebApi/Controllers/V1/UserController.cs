using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Application.Parameters;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {

        [HttpGet("get")]
        public async Task<IActionResult> GetUser([FromBody] string request)
        {
            return Ok();
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] RequestParameters filter)
        {
            return Ok();
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] string request)
        {
            return Ok();
        }

        [HttpPatch("toggle-active")]
        public async Task<IActionResult> ToggleUserActive([FromBody] string request)
        {
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] string request)
        {
            return Ok();
        }

    }
}
