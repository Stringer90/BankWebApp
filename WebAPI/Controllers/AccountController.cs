using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        // ===== FOR ACCOUNT MANAGEMENT PART OF TASK DESCRIPTION FOR TUTORIAL 7 =====

        // create account
        [HttpPost]
        [Route("createaccount/{userID}")]
        public IActionResult CreateAccount(int userID)
        {
            bool result = DBManager.CreateAccount(userID);
            if (result)
            {
                return Ok("Successfully created account"); // Return the new account object
            }
            return BadRequest("Error when creating account");
        }
        // retrieve account details by account number
        [HttpGet]
        [Route("getaccount/{accountNum}")]
        public IActionResult GetAccount(int accountNum)
        {
            Account account = DBManager.GetAccount(accountNum);

            // if account not found, account will be null, checked when getting request results
            return Json(account);
        }

        // update account details (amount / balance)
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

        // delete an account
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

        // ===== OTHER METHODS FOR USE IN TUTORIAL 8 APP =====

        // remove from balance (for use in transaction operations)
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


        // add to balance (for use in transaction operations)
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

        // get only accounts of a user
        // returns list of account objects
        // (for use in a list selection) (to select which to view)
        [HttpGet]
        [Route("getaccountsofuser/{userID}")]
        public IEnumerable<Account> GetAccountsOfUser(int userID)
        {
            List<Account> accounts = DBManager.GetAccountsOfUser(userID);

            // if account not found, account will be null, checked when getting request results
            return accounts;
        }

    }
}
