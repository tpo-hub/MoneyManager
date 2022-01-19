using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public interface IUserService
    {
        int GetUser();
    }
    public class UserService : IUserService
    {
        public int GetUser()
        {
            return 1;
        }
    }
}
