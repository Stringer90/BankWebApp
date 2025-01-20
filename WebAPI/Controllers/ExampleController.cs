using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Diagnostics;
//using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : Controller
    {
        [HttpGet]
        public IActionResult DeleteUser()
        {
            return Ok("Successfully deleted user");
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] Example? example)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns detailed validation errors.
            }

            if (example == null)
            {
                return BadRequest(new { message = "Body must not be null." });
            }

            if (string.IsNullOrWhiteSpace(example.name))
            {
                return BadRequest(new { message = "'name' must not be null or whitespace." });
            }

            if (example.id <= 0)
            {
                return BadRequest(new { message = "'id' must be a positive integer." });
            }

            // Proceed with creation logic.
            return Ok(new { message = "User created successfully." });




        }
    }
}
