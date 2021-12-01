using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class ResponseModel
    {
        public string StatusText { get; set; }
        public int StatusCode { get; set; }
        public string NewWalletId { get; set; }
        public decimal Amount { get; set; }
        public List<ThirdPartyApiResponse> TransactionList { get; set; }
    }
}
