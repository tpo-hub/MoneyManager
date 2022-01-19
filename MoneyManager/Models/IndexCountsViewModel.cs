using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class IndexCountsViewModel
    {
        public string CountType { get; set; }
        public IEnumerable<Count> Counts { get; set; }
        public bool Condition { get; set; }
        public decimal BalancePositive => Counts.Where(x=>x.Condition == true).Sum(x => x.Balance);
        public decimal BalanceNegative => Counts.Where(x => x.Condition == false).Sum(x => x.Balance);
    }
}
