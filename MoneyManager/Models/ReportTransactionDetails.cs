using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class ReportTransactionDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<TransactionsForDate> TransactionGroup { get; set; }
        public decimal BalanceDeposits => TransactionGroup.Sum(x => x.BalanceDeposits);
        public decimal BalanceRetirement => TransactionGroup.Sum(x => x.BalanceRetirement);
        public decimal Total => BalanceDeposits - BalanceRetirement;

        public class TransactionsForDate
        {
            public DateTime DateTransaction { get; set; }
            public IEnumerable<Transaction> Transactions { get; set; }
            public decimal BalanceDeposits =>
                Transactions.Where(x => x.Condition == true)
                .Sum(x => x.Mount);
            public decimal BalanceRetirement =>
                Transactions.Where(x => x.Condition == false)
                .Sum(x => x.Mount);
        }
    }
}
