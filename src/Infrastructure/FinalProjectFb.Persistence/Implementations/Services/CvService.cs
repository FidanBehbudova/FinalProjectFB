using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels.Cv;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.ViewModels;
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
	public class CvService:ICvService
	{
		private readonly ICvRepository _repository;
		private readonly IHttpContextAccessor _accessor;
		private readonly IUserService _user;

		public CvService(ICvRepository repository,IHttpContextAccessor accessor,IUserService user) 
        {
			_repository = repository;
			_accessor = accessor;
			_user = user;
		}
        //public async Task<PaginateVM<Cv>> GetAllAsync(int id, int page = 1, int take = 10)
        //{
        //    if (page < 1 || take < 1) throw new Exception("Bad request");

        //    // Filtreleme ekleyin
        //    ICollection<Cv> cvs = await _repository
        //        .GetPagination(
        //            filter: x => x.JobId == id, // Bu satırı ekleyin
        //            skip: (page - 1) * take,
        //            take: take,
        //            orderExpression: x => x.Id,
        //            IsDescending: true
        //        )
        //        .ToListAsync();

        //    // Diğer kodlar...
        //}

        public async Task<PaginateVM<Cv>> GetAllAsync(int id, int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new Exception("Bad request");
            ICollection<Cv> cvs = await _repository.GetPagination(skip: (page - 1) * take, take: take, orderExpression: x => x.Id, IsDescending: true).Where(x=>x.JobId==id).ToListAsync();
            if (cvs == null) throw new Exception("Not Found");
            int count = await _repository.GetAll().CountAsync();
            if (count < 0) throw new Exception("Not Found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<Cv> vm = new PaginateVM<Cv>
            {
                Items = cvs,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        public async Task<bool> CreateAsync(CreateCvVM vm, ModelStateDictionary modelstate)
		{

			if (!modelstate.IsValid) return false;			

			string username = "";
			if (_accessor.HttpContext.User.Identity != null)
			{
				username = _accessor.HttpContext.User.Identity.Name;
			}
			AppUser User = await _user.GetUser(username);
      

            if (vm != null)
            {
                
                if (vm.Name != null && vm.FatherName != null && vm.Surname != null
                    && vm.FinnishCode != null && vm.Address != null && vm.Birthday != null
                    && vm.PhoneNumber != null && vm.JobId > 0)
                {
                    
                    Cv cv = new Cv
                    {
                        AppUserId = User.Id,
                        CreatedBy = User.UserName,
                        CreatedAt = DateTime.UtcNow,
                        Name = vm.Name,
                        FatherName = vm.FatherName,
                        Surname = vm.Surname,
                        FinnishCode = vm.FinnishCode,
                        Address = vm.Address,
                        Birthday = vm.Birthday,
                        PhoneNumber = vm.PhoneNumber,
						EmailAddress = vm.EmailAddress,
                        JobId = vm.JobId
                    };

                   
                    await _repository.AddAsync(cv);
                    await _repository.SaveChangesAsync();
                    return true;
                }
            }

            return false;
		}
		//public async Task<bool> CreateAsync(CreateCvVM vm, ModelStateDictionary modelstate)
		//{
		//	if (!modelstate.IsValid) return false;

		//	AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);
		//	await _repository.AddAsync(new Cv
		//	{
		//		CreatedBy = User.UserName,
		//		CreatedAt = DateTime.UtcNow,
		//		Name = vm.Name,
		//		FatherName = vm.FatherName,
		//		Surname = vm.Surname,
		//		FinnishCode = vm.FinnishCode,
		//		Address = vm.Address,
		//		Birthday = vm.Birthday,
		//		PhoneNumber = vm.PhoneNumber,

		//		JobId=vm.JobId
		//	});
		//	await _repository.SaveChangesAsync();
		//	return true;
		//}
	}
}
