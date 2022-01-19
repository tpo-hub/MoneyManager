using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string EmailNormalize { get; set; }
        public string PasswordHsh { get; set; }

    }
}
