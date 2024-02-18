using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class HomeService:IHomeService
    {
        private readonly ICategoryRepository _category;
        private readonly IJobRepository _job;
        
        private readonly ICompanyRepository _company;

        public HomeService(ICategoryRepository category,IJobRepository job,ICompanyRepository company)
        {
            _category = category;
            _job = job;        
            _company = company;
        }
        public async Task<HomeVM> GetAllAsync()
        {
            List<Category> categories = await _category.GetAll().Include(c=>c.Jobs).ToListAsync();
            List<Job> jobs = await _job.GetAll()
                .Include(j=>j.Images)
                .Include(j=>j.Category)
                
                .Include(j => j.Company).ThenInclude(c=>c.CompanyCities).ThenInclude(c=>c.City)
                .ToListAsync();
           
            List<Company> companies = await _company.GetAll().Include(j => j.Images).ToListAsync();


            HomeVM homeVM = new HomeVM
            {
                Jobs = jobs,
                Categories = categories,               
                Companies = companies

            };
            return homeVM;

        }
    }
}
