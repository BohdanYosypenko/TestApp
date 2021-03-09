using Microsoft.AspNetCore.Identity;

namespace LegiosoftTestApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }        
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
