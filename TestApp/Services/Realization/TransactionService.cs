using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services.Interface;
using TestApp.ViewModels;

namespace TestApp.Services.Realization
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext _context;
        public TransactionService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            transaction.TransactionId = 0;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task AddRangeAsync(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (await _context.Transactions.AnyAsync(x => x.TransactionId == transaction.TransactionId))
                {
                    _context.Transactions.Update(transaction);
                }
                else
                {
                    transaction.TransactionId = 0;
                    await _context.Transactions.AddAsync(transaction);
                }
            }
            await _context.SaveChangesAsync();

        }

        public async Task<List<Transaction>> GetTransactionsAsync(TranactionFilrtrationQuery filter)
        {
            IQueryable<Transaction> query = _context.Transactions;

            if (filter != null)
            {
                var expressionsList = new List<Expression<Func<Transaction, bool>>>();

                if (filter.Status != null)
                {
                    Expression<Func<Transaction, bool>> statusFilter = a => a.Status == filter.Status;
                    expressionsList.Add(statusFilter);
                }

                if (filter.Type != null)
                {
                    Expression<Func<Transaction, bool>> userFilter = a => a.Type == filter.Type;
                    expressionsList.Add(userFilter);
                }

                if (filter.ClientName != null)
                {
                    Expression<Func<Transaction, bool>> nameFilter = a => a.ClientName.ToUpper().StartsWith(filter.ClientName.ToUpper());
                    expressionsList.Add(nameFilter);
                }

                Expression<Func<Transaction, bool>> expression = doctor => true;

                foreach (var exp in expressionsList)
                {
                    expression = expression.AndAlso(exp);
                }

                if (expression != null)
                    query = query.Where(expression);
            }
                        
            return await query.ToListAsync<Transaction>();
        }

        public async Task<Transaction> GetTransactionsByIdAsync(int id)
        {
            Transaction transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == id);

            return transaction;
        }

        public async Task<bool> UpdateAsync(Transaction transaction, int id)
        {
            if (await _context.Transactions.AnyAsync(x => x.TransactionId == id))
            {
                transaction.TransactionId = id;
                _context.Transactions.Update(transaction);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _context.Transactions.AnyAsync(x => x.TransactionId == id))
            {
                Transaction transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == id);
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
