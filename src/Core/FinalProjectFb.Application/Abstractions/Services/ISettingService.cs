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
    public interface ISettingService
    {
        Task<bool> CreateAsync(CreateSettingVM vm, ModelStateDictionary modelstate);

        Task<bool> UpdateAsync(int id, UpdateSettingVM vm, ModelStateDictionary modelstate);
        Task<UpdateSettingVM> UpdatedAsync(int id, UpdateSettingVM vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginateVM<Setting>> GetAllAsync(int page = 1, int take = 10);
    }
}
