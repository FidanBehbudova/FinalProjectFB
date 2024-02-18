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
    public interface ICategoryService
    {
        
        Task<bool> CreateAsync(CreateCategoryVM vm, ModelStateDictionary modelstate);
        
        Task<bool> UpdateAsync(int id, UpdateCategoryVM vm, ModelStateDictionary modelstate);
        Task<UpdateCategoryVM> UpdatedAsync(int id, UpdateCategoryVM vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginateVM<Category>> GetAllAsync(int page = 1, int take = 10);

    }
}
