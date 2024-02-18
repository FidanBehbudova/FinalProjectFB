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
using System.Xml.Linq;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _user;

        public CategoryService(ICategoryRepository repository, IHttpContextAccessor accessor,IUserService user)
        {
            _repository = repository;
            _accessor = accessor;
            _user = user;
        }
        public async  Task<bool> CreateAsync(CreateCategoryVM vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;

            if (await _repository.IsExistAsync(c => c.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This Category is already exist");
                return false;
            }
            AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);
            await _repository.AddAsync(new Category
            {
                CreatedBy = User.UserName,
                Name = vm.Name,
                Icon= vm.Icon,

            });
            await _repository.SaveChangesAsync();
            return true;
        }

        

        public async Task<PaginateVM<Category>> GetAllAsync(int page = 1, int take = 10)
        {
            ICollection<Category> categories = await _repository.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();
            int count = await _repository.GetAll().CountAsync();
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<Category> vm = new PaginateVM<Category>
            {
                Items = categories,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

       

        public async Task<bool> UpdateAsync(int id, UpdateCategoryVM vm, ModelStateDictionary modelstate)
		{
			if (id < 1) throw new ArgumentOutOfRangeException("id");
			if (!modelstate.IsValid) return false;

			Category exist = await _repository.GetByIdAsync(id);
			if (exist == null) throw new Exception("Not found");

			
			if (await _repository.IsExist(l => l.Name == vm.Name && l.Id != id))
			{
				modelstate.AddModelError("Name", "This Category is already exist");
				return false;
			}

			exist.Name = vm.Name;
			exist.Icon = vm.Icon;

			_repository.Update(exist);
			await _repository.SaveChangesAsync();
			return true;
		}




		public async Task<UpdateCategoryVM> UpdatedAsync(int id, UpdateCategoryVM vm)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");
            Category exist = await _repository.GetByIdAsync(id);
            if (exist == null) throw new Exception("Not found");
           
            vm.Name = exist.Name.Trim();
            vm.Icon = exist.Icon;
            return vm;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");
            Category exist = await _repository.GetByIdAsync(id);
            if (exist == null) throw new Exception("Not found");
            _repository.Delete(exist);
            await _repository.SaveChangesAsync();
            return true;
        }

        


    }
}
