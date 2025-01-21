﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        [HttpPost]
        [Route("createtransaction/{senderNum}/{receiverNum}/{amount}/{description}/{type}/{date}")]
        public IActionResult CreateTransaction(int? senderNum, int? receiverNum, double amount, string description, string type, string date)
        {
            try
            {
                Console.WriteLine($"Attempting transfer: From {senderNum} to {receiverNum}, Amount: {amount}, Type: {type}");

                if (senderNum.HasValue)
                {
                    var senderAccount = DBManager.GetAccount(senderNum.Value);
                    Console.WriteLine($"Sender account balance: {senderAccount?.Balance}");
                    if (senderAccount == null || senderAccount.Balance < amount)
                    {
                        return Json(new { message = $"Error: Insufficient funds or invalid sender account. Balance: {senderAccount?.Balance}, Amount: {amount}" });
                    }
                }

                if (DBManager.CreateTransaction(senderNum, receiverNum, amount, description, type, date))
                {
                    if (senderNum.HasValue)
                    {
                        Account senderAccount = DBManager.GetAccount(senderNum.Value);
                        DBManager.UpdateAccount(senderNum.Value, (senderAccount.Balance ?? 0) - amount);
                    }
                    if (receiverNum.HasValue)
                    {
                        Account receiverAccount = DBManager.GetAccount(receiverNum.Value);
                        DBManager.UpdateAccount(receiverNum.Value, (receiverAccount.Balance ?? 0) + amount);
                    }

                    return Json(new { message = "Successfully created transaction and updated balances" });
                }
                return Json(new { message = "Error when creating transaction" });
            }
            catch (Exception ex)
            {
                return Json(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("getacctrans/{accountNum}/{startDate}/{endDate}")]
        public IEnumerable<TransactionAccount> GetAccountTransactions(int accountNum, string startDate, string endDate)
        {
            List<TransactionAccount> transactions = DBManager.GetAccountTransactions(accountNum, startDate, endDate);

            return transactions;
        }

        [HttpGet]
        [Route("getalldatedesc/{searchInput}/{startDate}/{endDate}")]
        public IEnumerable<Models.Transaction> GetAllTransactionsDateDesc(string searchInput, string startDate, string endDate)
        {
            List<Models.Transaction> transactions = DBManager.GetAllTransactionsDateDesc(searchInput, startDate, endDate);

            return transactions;
        }

        [HttpGet]
        [Route("getalldateasc/{searchInput}/{startDate}/{endDate}")]
        public IEnumerable<Models.Transaction> GetAllTransactionsDateAsc(string searchInput, string startDate, string endDate)
        {
            List<Models.Transaction> transactions = DBManager.GetAllTransactionsDateAsc(searchInput, startDate, endDate);

            return transactions;
        }
    }
}
