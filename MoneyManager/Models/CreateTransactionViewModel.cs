using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class CreateTransactionViewModel : Transaction
    {
        public IEnumerable<SelectListItem> Counts { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
