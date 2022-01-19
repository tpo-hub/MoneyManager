using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Tipo operacion es requerido")]
        [Display(Name = "Tipo Operación")]
        public OperationType TransactionTypeId { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
