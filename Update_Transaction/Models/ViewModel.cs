using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class ViewModel
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public int Amount { get; set; }
        public string TransactionType { get; set; }
        public string WalletAddress { get; set; }
        public decimal Balance { get; set; }
        public List<ThirdPartyApiResponse> TransactionList { get; set; }
    }

    public class ThirdPartyApiResponse
    {
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionDate { get; set; }


    }
}
