using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        [HttpPost]
        [Route("createaccount/{userID}")]
        public IActionResult CreateAccount(int userID)
        {
            bool result = DBManager.CreateAccount(userID);
            if (result)
            {
                return Ok("Successfully created account"); 
            }
            return BadRequest("Error when creating account");
        }

        [HttpGet]
        [Route("getaccount/{accountNum}")]
        public IActionResult GetAccount(int accountNum)
        {
            Account account = DBManager.GetAccount(accountNum);

            return Json(account);
        }

        [HttpPost]
        [Route("updateaccount/{accountNum}/{newBalance}")]
        public IActionResult UpdateAccount(int accountNum, double newBalance)
        {
            if (DBManager.UpdateAccount(accountNum, newBalance))
            {
                return Ok("Successfully updated account balance");
            }
            return BadRequest("Error when updating account balance");
        }

        [HttpPost]
        [Route("deleteaccount/{accountNum}")]
        public IActionResult DeleteAccount(int accountNum)
        {
            if (DBManager.DeleteAccount(accountNum))
            {
                return Ok("Successfully deleted account");
            }
            return BadRequest("Error when deleting account");
        }

        [HttpPost]
        [Route("accountwithdraw/{accountNum}/{amount}")]
        public IActionResult AccountWithdraw(int accountNum, double amount)
        {
            if (DBManager.AccountWithdraw(accountNum, amount))
            {
                return Ok("Successfully withdrew from account");
            }
            return BadRequest("Error when withdrawing from account");
        }

        [HttpPost]
        [Route("accountdeposit/{accountNum}/{amount}")]
        public IActionResult AccountDeposit(int accountNum, double amount)
        {
            if (DBManager.AccountDeposit(accountNum, amount))
            {
                return Ok("Successfully deposited into account");
            }
            return BadRequest("Error when withdrawing from account");
        }

        [HttpGet]
        [Route("getaccountsofuser/{userID}")]
        public IEnumerable<Account> GetAccountsOfUser(int userID)
        {
            List<Account> accounts = DBManager.GetAccountsOfUser(userID);

            return accounts;
        }

    }
}
