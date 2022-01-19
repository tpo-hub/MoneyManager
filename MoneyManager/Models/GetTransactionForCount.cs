using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class GetTransactionForCount
    {
        public int UserId { get; set; }
        public int CountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
