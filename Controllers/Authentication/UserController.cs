using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyDMS.Domain;

namespace MyDMS.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet("all-users")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult GetUsers()
        {
            return Ok("These are all the users");
        }

    }

}
