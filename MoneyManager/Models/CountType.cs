using Microsoft.AspNetCore.Mvc;
using MoneyManager.Models.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class CountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(maximumLength:50, MinimumLength = 4, ErrorMessage ="La longitud del campo Nombre debe ser mayor o igual a 4 ")]
        [Display(Name ="Nombre del tipo cuenta")]
        [FirstCaseUpper]
       // [Remote(action: "VerifyExistCountType", controller: "CountsTypeController")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int OrderType { get; set; }
    }
}
