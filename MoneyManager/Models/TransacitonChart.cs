using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class TransacitonChart
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Mount { get; set; }
        public string Date { get; set; }
        public bool Condition { get; set; }
    }
}
