using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class Transaction : BaseModel
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
