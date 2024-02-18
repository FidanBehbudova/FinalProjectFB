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
    public interface ICompanyService
    {
        Task<PaginateVM<Company>> GetCompaniesCreatedByUserAsync(string userId, int page = 1, int take = 10);
        Task<UpdateCompanyVM> GetCitiesForUpdateFormAsy(UpdateCompanyVM updateCompanyVM);

        Task<bool> GetConfirmationFormAsync(ConfirmationFormVM vm,ModelStateDictionary modelstate);
        Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM);
        //void ReverseDeleteCompany(int companyId);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        //Task<bool> DeletedFormStatus(int id);
        Task<PaginateVM<Company>> GetAllAsync(int page = 1, int take = 10);
        Task<CompanyDetailVM> DetailAsync(int id);
        Task<UpdateCompanyVM> UpdatedAsync(int id);
        Task<bool> UpdateAsync(UpdateCompanyVM updateCompanyVM, ModelStateDictionary modelState, int id);
    }
}
