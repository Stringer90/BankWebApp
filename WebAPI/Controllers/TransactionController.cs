using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        [HttpPost]
        [Route("create")]
        public IActionResult CreateTransaction([FromBody] Transaction? transaction)
        {
            try
            {
                if (transaction is null)
                {
                    return BadRequest("A 'Transaction' object must be provided.");
                }
                else if (string.IsNullOrWhiteSpace(transaction.Date))
                {
                    return BadRequest("Transaction Date cannot be null or whitespace.");
                }
                else if (transaction.Sender is null)
                {
                    return BadRequest("Transaction Sender cannot be null.");
                }
                else if (transaction.Receiver is null)
                {
                    return BadRequest("Transaction Receiver cannot be null.");
                }
                else if (string.IsNullOrWhiteSpace(transaction.Type))
                {
                    return BadRequest("Transaction Type cannot be null or whitespace.");
                }
                else if (transaction.Amount is null)
                {
                    return BadRequest("Transaction Amount cannot be null.");
                }
                else if (transaction.Amount <= 0)
                {
                    return BadRequest("Transaction Amount cannot be zero or a negative number.");
                }
                else if (string.IsNullOrWhiteSpace(transaction.Description))
                {
                    return BadRequest("Transaction Description cannot be null or whitespace.");
                }

                Account? receiverAccount = DBManager.GetAccount(transaction.Receiver.Value);
                if (receiverAccount is null)
                {
                    return BadRequest($"Transaction receiver account {transaction.Receiver} does not exist.");
                }

                Account? senderAccount = DBManager.GetAccount(transaction.Sender.Value);
                if (senderAccount is null)
                {
                    return BadRequest($"Transaction sender account {transaction.Sender} does not exist.");
                }
                else if (senderAccount.Balance < transaction.Amount)
                {
                    return BadRequest($"Transaction sender account {transaction.Receiver} does not have the necessary funds to perform the transaction.");
                }

                if (!Regex.IsMatch(transaction.Date, @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$")) {
                    return BadRequest($"Transaction date {transaction.Date} is an invalid date or invalid format.");
                }
                else if (new[] {"deposit", "withdrawal", "transfer"}.Contains(transaction.Type))
                {
                    return BadRequest("Transaction type can only be 'deposit', 'withdrawal' or 'transfer'.");
                }

                DBManager.AccountWithdraw(transaction.Sender.Value, transaction.Amount.Value);
                DBManager.AccountDeposit(transaction.Receiver.Value, transaction.Amount.Value);
                DBManager.CreateTransaction(transaction.Sender.Value, transaction.Receiver.Value, transaction.Amount.Value, transaction.Description, transaction.Type, transaction.Date);
                
                return Ok("Successfully completed the transaction.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{accountNum}")]
        public IActionResult GetAccountTransactions(int? accountNum)
        {
            if (accountNum is null)
            {
                return BadRequest("Account number cannot be null.");
            }

            Account? account = DBManager.GetAccount(accountNum.Value);
            if (account is null)
            {
                return BadRequest($"An account {accountNum.Value} does not exist.");
            }

            List<TransactionAccount> transactions = DBManager.GetAccountTransactions(accountNum.Value);

            return Ok(transactions);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllTransactions()
        {
            List<Models.Transaction> transactions = DBManager.GetAllTransactions();

            return Ok(transactions);
        }
    }
}
