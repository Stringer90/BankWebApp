using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;
using System.Diagnostics.Eventing.Reader;
using System.Security.Principal;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        [HttpPost]
        [Route("create")]
        public IActionResult CreateAccount([FromBody] int? userID)
        {
            try
            {
                if (userID is null)
                {
                    return BadRequest("User ID cannot be null.");
                }

                if (DBManager.GetUserWithID(userID.Value) is null)
                {
                    return BadRequest($"No user with an ID of {userID} exists.");
                }

                if (DBManager.CreateAccount(userID.Value))
                {
                    return Ok("Successfully created account");
                }

                return BadRequest("Error when creating account.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{accountNum}")]
        public IActionResult GetAccount(int? accountNum)
        {
            try
            {
                if (accountNum is null)
                {
                    return BadRequest("Account number cannot be null.");
                }
                else if (accountNum.Value < 10000 || accountNum.Value > 99999)
                {
                    return BadRequest("Account number is a 5-digit number starting at 10,000.");
                }

                Account? account = DBManager.GetAccount(accountNum);

                if (account is null)
                {
                    return BadRequest($"Account {accountNum} does not exist.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateAccount([FromBody] Account? account)
        {
            try
            {
                if (account is null)
                {
                    return BadRequest("An 'Account' object must be provided.");
                }
                else if (account.AccountNum is null)
                {
                    return BadRequest("Account number cannot be null");
                }
                else if (account.AccountNum.Value < 10000 || account.AccountNum.Value > 99999)
                {
                    return BadRequest("Account number is a 5-digit number starting at 10,000.");
                }
                else if (account.Balance is null)
                {
                    return BadRequest("New balance cannot be null.");
                }
                else if (account.Balance < 0)
                {
                    return BadRequest("New balance cannot be a negative value.");
                }

                Account? userAccount = DBManager.GetAccount(account.AccountNum);

                if (userAccount is null)
                {
                    return BadRequest($"Account {account.AccountNum} does not exist.");
                }

                if (DBManager.UpdateAccount(account.AccountNum, account.Balance.Value))
                {
                    return Ok("Successfully updated account balance");
                }

                return BadRequest("Error when updating account balance");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteAccount([FromBody] int? accountNum)
        {
            try
            {
                if (accountNum is null)
                {
                    return BadRequest("Account number cannot be null.");
                }
                else if (accountNum.Value < 10000 || accountNum.Value > 99999)
                {
                    return BadRequest("Account number is a 5-digit number starting at 10,000.");
                }

                Account? account = DBManager.GetAccount(accountNum);

                if (account is null)
                {
                    return BadRequest($"Account {accountNum} does not exist.");
                }

                if (DBManager.DeleteAccount(accountNum.Value))
                {
                    return Ok("Successfully deleted account");
                }

                return BadRequest("Error when deleting account");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("withdraw")]
        public IActionResult AccountWithdraw([FromBody] Account? account)
        {
            try { 
                if (account is null)
                {
                    return BadRequest("An 'Account' object must be provided.");
                }
                else if (account.AccountNum < 10000 || account.AccountNum > 99999)
                {
                    return BadRequest("Account number is a 5-digit number starting at 10,000.");
                }
                else if (account.AccountNum is null)
                {
                    return BadRequest("Account number cannot be null");
                }
                else if (account.Balance is null)
                {
                    return BadRequest("New balance cannot be null.");
                }
                else if (account.Balance <= 0)
                {
                    return BadRequest("Amount to withdraw cannot be zero or a negative value.");
                }

                Account? userAccount = DBManager.GetAccount(account.AccountNum);

                if (userAccount is null)
                {
                    return BadRequest($"Account {account.AccountNum} does not exist.");
                }

                if (userAccount.Balance < account.Balance)
                {
                    return BadRequest("Account doesn't have the balance necessary to perform the withdrawal.");
                }

                if (DBManager.AccountWithdraw(account.AccountNum.Value, account.Balance.Value))
                {
                    return Ok("Successfully withdrew from account");
                }

                return BadRequest("Error when withdrawing from account");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("deposit")]
        public IActionResult AccountDeposit([FromBody] Account? account)
        {
            try { 
                if (account is null)
                {
                    return BadRequest("An 'Account' object must be provided.");
                }
                else if (account.AccountNum is null)
                {
                    return BadRequest("Account number cannot be null");
                }
                else if (account.AccountNum < 10000 || account.AccountNum > 99999)
                {
                    return BadRequest("Account number is a 5-digit number starting at 10,000.");
                }
                else if (account.Balance is null)
                {
                    return BadRequest("New balance cannot be null.");
                }
                else if (account.Balance <= 0)
                {
                    return BadRequest("Cannot deposit nothing or a negative amount.");
                }

                Account? userAccount = DBManager.GetAccount(account.AccountNum);

                if (userAccount is null)
                {
                    return BadRequest($"Account {account.AccountNum} does not exist.");
                }

                if (DBManager.AccountDeposit(account.AccountNum.Value, account.Balance.Value))
                {
                    return Ok("Successfully deposited into account");
                }

                return BadRequest("Error when withdrawing from account");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{userID}/accounts")]
        public IActionResult GetAccountsOfUser(int? userID)
        {
            try
            {
                if (userID is null)
                {
                    return BadRequest("User ID cannot be null.");
                }

                if (DBManager.GetUserWithID(userID.Value) is null)
                {
                    return BadRequest($"No user with an ID of {userID} exists.");
                }

                List<Account> accounts = DBManager.GetAccountsOfUser(userID.Value);
                
                if (accounts == null || !accounts.Any())
                {
                    return BadRequest($"No accounts for user {userID.Value} found.");
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
