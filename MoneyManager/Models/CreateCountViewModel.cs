using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class CreateCountViewModel : Count
    {
        public IEnumerable<SelectListItem> CountsType { get; set; }
    }
}
