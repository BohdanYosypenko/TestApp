using ClosedXML.Excel;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services.Interface;
using TestApp.ViewModels;

namespace TestApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {

        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {

            _transactionService = transactionService;
        }
        /// <summary>
        /// accepts the filter and returns all transactions
        /// </summary>
        /// <param name="filter"> filter parameters: Status, Type, ClientName</param>
        /// <returns>returns all transactions</returns>
        [HttpGet("/transaction")]
        public async Task<IActionResult> GetAllTransaction([FromQuery] TranactionFilrtrationQuery filter)
        {
            return Ok(await _transactionService.GetTransactionsAsync(filter));
        }
        /// <summary>
        /// accepts the filter and returns all transactions in excel file
        /// </summary>
        /// <returns>returns excel file with filtered transactions</returns>
        [HttpGet("/transaction/file")]
        public async Task<IActionResult> GetAllTransactionByFile([FromQuery] TranactionFilrtrationQuery filter)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transaction");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "TransactionId";
                worksheet.Cell(currentRow, 2).Value = "Status";
                worksheet.Cell(currentRow, 3).Value = "Type";
                worksheet.Cell(currentRow, 4).Value = "ClientName";
                worksheet.Cell(currentRow, 5).Value = "Amount";

                List<Transaction> transactions = await _transactionService.GetTransactionsAsync(filter);
                foreach (var transaction in transactions)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = transaction.TransactionId;
                    worksheet.Cell(currentRow, 2).Value = transaction.Status;
                    worksheet.Cell(currentRow, 3).Value = transaction.Type;
                    worksheet.Cell(currentRow, 4).Value = transaction.ClientName;
                    worksheet.Cell(currentRow, 5).Value = "$" + transaction.Amount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                         "Transaction.xlsx");
                }
            }
        }
        /// <summary>
        /// get transaction by id
        /// </summary>
        /// <param name="id">TransactionId</param>
        /// <returns>returns transaction with selected id</returns>
        [HttpGet("/transaction/{id}")]
        public async Task<IActionResult> GetTransactionById([FromRoute] int id)
        {
            var transaction = await _transactionService.GetTransactionsByIdAsync(id);
            if (transaction != null)
                return Ok(transaction);
            else
                return NotFound();
        }

        /// <summary>
        /// Merge transaction from file 
        /// </summary>
        /// <param name="uploadedFile">csv file is neaded</param>
        /// <returns>returns status of operation</returns>
        [HttpPost("/transaction/file")]
        public async Task<IActionResult> PostTransactionByFile(IFormFile uploadedFile)
        {
            try
            {
                IEnumerable<TransactionViewModel> records;
                List<Transaction> transactions;
                using (var reader = new StreamReader(uploadedFile.OpenReadStream()))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        records = csv.GetRecords<TransactionViewModel>().ToList();
                        transactions = new List<Transaction>();
                        foreach (var transactionViewModel in records)
                        {
                            Transaction transaction = new Transaction()
                            {
                                TransactionId = transactionViewModel.TransactionId,
                                Status = transactionViewModel.Status,
                                Type = transactionViewModel.Type,
                                ClientName = transactionViewModel.ClientName,
                                Amount = Convert.ToDecimal(transactionViewModel.Amount.Trim('$'))
                            };
                            transactions.Add(transaction);
                        }
                        await _transactionService.AddRangeAsync(transactions);

                    }
                    return Ok();
                }
            }
            catch
            {
                return BadRequest(); 
            }
        }

        /// <summary>
        /// Add transaction to database
        /// </summary>
        /// <param name="transaction">added transaction</param>
        /// <returns>return added transaction</returns>
        [HttpPost("/transaction")]
        public async Task<IActionResult> PostTransaction([FromBody] Transaction transaction)
        {
            var res = await _transactionService.AddAsync(transaction);
            return Ok(res);
        }

        /// <summary>
        /// Update transaction with selected id
        /// </summary>
        /// <param name="transaction">new transaction</param>
        /// <param name="id">id of updated transaction</param>
        /// <returns>status for update operation</returns>
        [HttpPut("/transaction/{id}")]
        public async Task<IActionResult> PutTransaction([FromBody] Transaction transaction, [FromRoute] int id)
        {
            var res = await _transactionService.UpdateAsync(transaction, id);
            if (res)
                return Ok();
            else
                return NotFound();
        }

        /// <summary>
        /// delete transaction with selected id
        /// </summary>
        /// <param name="id">id of deleted transaction</param>
        /// <returns>status of deleted operation</returns>
        [HttpDelete("/transaction/{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
        {
            var res = await _transactionService.DeleteAsync(id);
            if (res)
                return NoContent();
            else
                return NotFound();
        }

    }
}
