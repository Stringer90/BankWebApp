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
        [Route("create")]
        public IActionResult CreateAccount([FromBody] int userID)
        {
            bool result = DBManager.CreateAccount(userID);
            if (result)
            {
                return Ok("Successfully created account"); 
            }
            return BadRequest("Error when creating account");
        }

        [HttpGet]
        [Route("{accountNum}")]
        public IActionResult GetAccount(int accountNum)
        {
            Account account = DBManager.GetAccount(accountNum);

            return Json(account);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            if (DBManager.UpdateAccount(accountNum, newBalance))
            {
                return Ok("Successfully updated account balance");
            }
            return BadRequest("Error when updating account balance");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteAccount([FromBody] int accountNum)
        {
            if (DBManager.DeleteAccount(accountNum))
            {
                return Ok("Successfully deleted account");
            }
            return BadRequest("Error when deleting account");
        }

        [HttpPost]
        [Route("withdraw")]
        public IActionResult AccountWithdraw([FromBody] Account account)
        {
            if (DBManager.AccountWithdraw(accountNum, amount))
            {
                return Ok("Successfully withdrew from account");
            }
            return BadRequest("Error when withdrawing from account");
        }

        [HttpPost]
        [Route("deposit")]
        public IActionResult AccountDeposit([FromBody] Account acount)
        {
            if (DBManager.AccountDeposit(accountNum, amount))
            {
                return Ok("Successfully deposited into account");
            }
            return BadRequest("Error when withdrawing from account");
        }

        [HttpGet]
        [Route("{userID}/accounts")]
        public IEnumerable<Account> GetAccountsOfUser(int userID)
        {
            List<Account> accounts = DBManager.GetAccountsOfUser(userID);

            return accounts;
        }

    }
}
