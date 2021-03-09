using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.ViewModels;

namespace TestApp.Services.Interface
{
    public interface ITransactionService
    {
        public Task AddRangeAsync(IEnumerable<Transaction> transactions);

        public Task<List<Transaction>> GetTransactionsAsync(TranactionFilrtrationQuery filter);
        public Task<Transaction> GetTransactionsByIdAsync(int id);
        public Task<Transaction> AddAsync(Transaction transaction);
        public Task<bool> UpdateAsync(Transaction transaction, int id);
        public Task<bool> DeleteAsync(int id);
    }
}
