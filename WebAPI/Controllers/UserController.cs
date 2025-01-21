using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("createuser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (DBManager.CreateUser(user.Username, user.Password, user.Email, user.Address, user.Phone))
            {
                return Ok(new { message = "Successfully created user" });
            }
            return BadRequest(new { message = "Error when creating user" });
        }

        [HttpGet]
        [Route("getuser/{searchString}")]
        public IActionResult GetUser(string searchString)
        {
            User user = DBManager.GetUser(searchString);

            return Json(user);
        }

        [HttpPost]
        [Route("updateuser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            string username = user.Username;
            string password = user.Password;
            string email = user.Email;
            string address = user.Address;
            string phone = user.Phone;
            int? userID = user.UserID;
            if (DBManager.UpdateUser(user.Username, user.Password, user.Email, user.Address, user.Phone, (int)user.UserID))
            {
                return Ok(new { message = "Successfully updated user" });
            }
            return BadRequest(new { error = "Error when updating user" });
        }

        [HttpPost]
        [Route("deleteuserid/{userID}")]
        public IActionResult DeleteUser(int userID)
        {
            if (DBManager.DeleteUser(userID))
            {
                return Ok("Successfully deleted user");
            }
            return BadRequest("Error when deleting user");
        }

        [HttpGet]
        [Route("isvalidauth/{usernameOrEmail}/{password}")]
        public IActionResult IsValidAuth(string usernameOrEmail, string password)
        {
            bool isValid = DBManager.IsValidAuth(usernameOrEmail, password);
            if (isValid)
            {
                HttpContext.Session.SetString("User", usernameOrEmail);
            }
            return Json(isValid);
        }

        /*
        [HttpGet]
        [Route("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
        */

    }
}
