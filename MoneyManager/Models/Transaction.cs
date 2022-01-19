using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        public DateTime DateTransaction { get; set; } 
        public decimal Mount { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede pasar de {1} caracteres")]
        [Display(Name = "Notas")]
        public string Note { get; set; }

        [Display(Name = "Cuenta asociada")]
        public int CountId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Tipo Operación")]
        public OperationType TransactionType { get; set; } = OperationType.Ingreso;

        public bool Condition { get; set; } = true;
        public string Count { get; set; }
        public string Category { get; set; }




    }
}
