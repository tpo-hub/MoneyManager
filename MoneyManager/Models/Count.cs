using MoneyManager.Models.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Count
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "La longitud del campo Nombre debe ser mayor o igual a 4 ")]
        [FirstCaseUpper]
        [Display(Name = "Nombre de cuenta")]
        public string Name { get; set; }
        [Display(Name = "Tipo Cuenta asociado")]
        public int CountTypeId { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Debe tener aunque sea un monto de balance")]
        [Required(ErrorMessage = "El campo Balance es requerido")]
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 4, ErrorMessage = "La longitud del campo Descripcion debe ser mayor o igual a 4 ")]
        [FirstCaseUpper]
        [Display(Name = "Descripcion de cuenta")]
        public string Description { get; set; }
        public string CountType { get; set; }
       
        [Display(Name = "Condicion de la cuenta")]
        [Required(ErrorMessage = "El campo Condicion es requerido")]
        public bool Condition { get; set; }
    }
}
