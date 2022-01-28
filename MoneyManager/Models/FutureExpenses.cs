using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
  public class FutureExpenses
    {
        public string Name { get; set; }
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date{ get; set; }
        public decimal Mount { get; set; }
        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede pasar de {1} caracteres")]
        [Display(Name = "Notas")]
        public string Description { get; set; }
        [Display(Name = "Cuenta asociada")]
        public int CountId { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }
    }
}
