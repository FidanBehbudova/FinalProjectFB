using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class CityService : ICityService
    {
        private readonly ICityRepository _repository;
        private readonly IUserService _user;
        private readonly IHttpContextAccessor _accessor;

        public CityService(ICityRepository repository,IUserService user,IHttpContextAccessor accessor)
        {
            _repository = repository;
            _user = user;
            _accessor = accessor;
        }
        public async Task<bool> CreateAsync(CreateCityVM vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;

            if (await _repository.IsExistAsync(c => c.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This City is already exist");
                return false;
            }
            AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);
            await _repository.AddAsync(new City
            {
                
                CreatedBy = User.UserName,
                Name = vm.Name,
              

            });
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");
            City exist = await _repository.GetByIdAsync(id);
            if (exist == null) throw new Exception("Not found");
            _repository.Delete(exist);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<PaginateVM<City>> GetAllAsync(int page = 1, int take = 10)
        {
            ICollection<City> cities = await _repository.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();
            int count = await _repository.GetAll().CountAsync();
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<City> vm = new PaginateVM<City>
            {
                Items = cities,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCityVM vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");
            if (!modelstate.IsValid) return false;

            City exist = await _repository.GetByIdAsync(id);
            if (exist == null) throw new Exception("Not found");


            if (await _repository.IsExist(l => l.Name == vm.Name && l.Id != id))
            {
                modelstate.AddModelError("Name", "This City is already exist");
                return false;
            }

            exist.Name = vm.Name.Trim();
            

            _repository.Update(exist);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<UpdateCityVM> UpdatedAsync(int id, UpdateCityVM vm)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");
            City exist = await _repository.GetByIdAsync(id);
            if (exist == null) throw new Exception("Not found");

            vm.Name = exist.Name.Trim();
           
            return vm;
        }
    }
}
