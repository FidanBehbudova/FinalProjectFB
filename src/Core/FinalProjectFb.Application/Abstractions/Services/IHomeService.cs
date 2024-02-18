using FinalProjectFb.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface IHomeService
    {
        Task<HomeVM> GetAllAsync();
    }
}
