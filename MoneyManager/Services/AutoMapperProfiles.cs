using AutoMapper;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Count, CreateCountViewModel>();                
            CreateMap<Transaction, CreateTransactionViewModel>().ReverseMap();                
        }
    }
}
