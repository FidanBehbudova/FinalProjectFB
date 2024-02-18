using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface ICityService
    {
        Task<bool> CreateAsync(CreateCityVM vm, ModelStateDictionary modelstate);

        Task<bool> UpdateAsync(int id, UpdateCityVM vm, ModelStateDictionary modelstate);
        Task<UpdateCityVM> UpdatedAsync(int id, UpdateCityVM vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginateVM<City>> GetAllAsync(int page = 1, int take = 10);
    }
}
