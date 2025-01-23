using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser([FromBody] User? user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest("A 'User' object must be provided.");
                }
                else if (string.IsNullOrWhiteSpace(user.Username))
                {
                    return BadRequest("Username cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest("Password cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Email cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Address))
                {
                    return BadRequest("Address cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Phone))
                {
                    return BadRequest("Phone cannot be null or whitespace.");
                }
                else if (!Regex.IsMatch(user.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return BadRequest("Email format is not valid.");
                }
                else if (user.Phone.Length > 10 || !Regex.IsMatch(user.Phone, @"^\d+$"))
                {
                    return BadRequest("Phone number is invalid.");
                }

                // See if username, email or phone number is taken
                if (DBManager.GetUser(user.Username) is not null)
                {
                    return BadRequest("Username is already in use.");
                }
                else if (DBManager.GetUser(user.Email) is not null)
                {
                    return BadRequest("Email is already in use.");
                }
                else if (DBManager.GetUser(user.Phone) is not null)
                {
                    return BadRequest("Phone number is already in use.");
                }

                if (DBManager.CreateUser(user.Username, user.Password, user.Email, user.Address, user.Phone))
                {
                    return Ok("Successfully created user");
                }

                return BadRequest("Error when creating user");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{searchString}")]
        public IActionResult GetUser(string searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchString))
                {
                    return BadRequest("Username or email cannot be null or whitespace.");
                }

                User? user = DBManager.GetUser(searchString);

                if (user is null)
                {
                    return BadRequest($"A user with a username or email '{searchString}' does not exist.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateUser([FromBody] User? user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest("A 'User' object must be provided.");
                }
                else if (user.UserID is null)
                {
                    return BadRequest("UserID cannot be null.");
                }
                else if (string.IsNullOrWhiteSpace(user.Username))
                {
                    return BadRequest("Username cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest("Password cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Email cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Address))
                {
                    return BadRequest("Address cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Phone))
                {
                    return BadRequest("Phone cannot be null or whitespace.");
                }
                else if (!Regex.IsMatch(user.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return BadRequest("Email format is not valid.");
                }
                else if (user.Phone.Length > 10 || !Regex.IsMatch(user.Phone, @"^\d+$"))
                {
                    return BadRequest("Phone number is invalid.");
                }

                if (DBManager.GetUserWithID(user.UserID.Value) is null)
                {
                    return BadRequest($"No user with an ID of {user.UserID.Value} exists.");
                }

                if (DBManager.UpdateUser(user.Username, user.Password, user.Email, user.Address, user.Phone, user.UserID.Value))
                {
                    return Ok("Successfully updated user");
                }

                return BadRequest("Error when updating user");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteUser([FromBody] int? userID)
        {
            try
            {
                if (userID is null)
                {
                    return BadRequest("User ID cannot be null.");
                }
                else if (DBManager.GetUserWithID(userID.Value) is null)
                {
                    return BadRequest($"No user with an ID of {userID} exists.");
                }

                if (DBManager.DeleteUser(userID.Value))
                {
                    return Ok("Successfully deleted user");
                }

                return BadRequest("Error when deleting user");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        // Username field can contain the username or email of the login.
        [HttpPost]
        [Route("auth")]
        public IActionResult IsValidAuth([FromBody] User user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest("A 'User' object must be provided.");
                }
                else if (string.IsNullOrWhiteSpace(user.Username))
                {
                    return BadRequest("Username/email cannot be null or whitespace.");
                }
                else if (string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest("Password cannot be null or whitespace.");
                }

                return Ok(DBManager.IsValidAuth(user.Username, user.Password));
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
            /*
            bool isValid = DBManager.IsValidAuth(usernameOrEmail, password);
            if (isValid)
            {
                HttpContext.Session.SetString("User", usernameOrEmail);
            }
            return Json(isValid);
            */
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
