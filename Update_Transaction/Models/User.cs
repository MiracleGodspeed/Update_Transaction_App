using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class User : BaseModel
    {
        public string WalletID { get; set; }
        [MaxLength(100)]
        public string Fullname { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        public decimal UserBalance { get; set; }
    }
}
