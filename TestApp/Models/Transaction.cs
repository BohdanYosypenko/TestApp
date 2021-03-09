using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string ClientName { get; set; }

    }
}
