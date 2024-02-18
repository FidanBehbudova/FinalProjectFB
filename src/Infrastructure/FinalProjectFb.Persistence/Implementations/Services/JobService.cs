using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using FinalProjectFb.Persistence.Implementations.Repositories;
using FinalProjectFb.Persistence.Implementations.Repositories.Generic;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinalProjectFb.Persistence.Implementations.Services
{
	internal class JobService : IJobService
	{
		private readonly IJobRepository _repository;
		private readonly IHttpContextAccessor _accessor;
		private readonly IUserService _user;
		private readonly IWebHostEnvironment _env;
		private readonly ICompanyRepository _company;
		private readonly ICategoryRepository _category;

		public JobService(IJobRepository repository, IHttpContextAccessor accessor, IUserService user, IWebHostEnvironment env, ICompanyRepository company, ICategoryRepository category)
		{
			_repository = repository;
			_accessor = accessor;
			_user = user;
			_env = env;
			_company = company;
			_category = category;
		}
		
		public async Task<bool> Create(CreateJobVM createJobVM, ModelStateDictionary modelstate)
		{

			if (!modelstate.IsValid) return false;

			if (createJobVM.CategoryId is not null)
			{
				if (!await _category.IsExist(x => x.Id == createJobVM.CategoryId))
				{
					modelstate.AddModelError("CategoryId", "You dont have this Category");
					return false;
				}

			}

			string username = "";
			if (_accessor.HttpContext.User.Identity != null)
			{
				username = _accessor.HttpContext.User.Identity.Name;
			}
			AppUser User = await _user.GetUser(username);

			
			if (createJobVM.Photo != null)
			{
				if (!createJobVM.Photo.ValidateType("image/"))
				{
					modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!createJobVM.Photo.ValidateSize(600))
				{
					modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
					return false;
				}
			}

			Image photo = new Image
			{
				IsPrimary = true,
				Url = await createJobVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon")
			};

			Job job = new Job
			{
				Name = createJobVM.Name,
				Requirement = createJobVM.Requirement,
				JobNature = createJobVM.JobNature,
				Experience = createJobVM.Experience,
				Deadline = createJobVM.Deadline,
				Salary = createJobVM.Salary,
				Vacancy = createJobVM.Vacancy,
				AppUserId = User.Id,
				CreatedBy = User.UserName,
				CategoryId = createJobVM.CategoryId,
				CreatedAt = DateTime.UtcNow,
				CompanyId = createJobVM.CompanyId,
				Images = new List<Image> { photo }
			};
			


			await _repository.AddAsync(job);
			await _repository.SaveChangesAsync();
			return true;
		}
		public async Task<PaginateVM<Job>> GetAllAsync(int id, int page = 1, int take = 5)
		{
			if (page < 1 || take < 1) throw new Exception("Bad request");
			ICollection<Job> jobs = await _repository.GetPagination(skip: (page - 1) * take, take: take, orderExpression: x => x.Id, IsDescending: true, includes: nameof(Company)).Where(c => c.CompanyId == id).ToListAsync();
			if (jobs == null) throw new Exception("Not Found");
			int count = await _repository.GetAll().CountAsync();
			if (count < 0) throw new Exception("Not Found");
			double totalpage = Math.Ceiling((double)count / take);
			PaginateVM<Job> vm = new PaginateVM<Job>
			{
				Items = jobs,
				CurrentPage = page,
				TotalPage = totalpage
			};
			return vm;
		}

		public async Task<CreateJobVM> CreatedAsync(CreateJobVM vm)
		{

			vm.Categories = await _category.GetAll().ToListAsync();


			return vm;
		}

		public async Task<JobDetailVM> DetailAsync(int id)
		{
			if (id < 1) throw new ArgumentOutOfRangeException("id");

			Job job = await _repository.GetByIdAsync(id, includes: new string[] { "Company", "Company.CompanyCities", "Company.CompanyCities.City", "Category", "Images" });


			JobItemVM jobItemVM = new JobItemVM
			{
				JobId=job.Id,
				Vacancy = job.Vacancy,
				JobNature = job.JobNature,
				Requirement = job.Requirement,
				Name = job.Name,
				CreatedAt = job.CreatedAt,
				Category = job.Category,
				Deadline = job.Deadline,				
				Salary = job.Salary,
				Experience = job.Experience,
				Company = job.Company,
				Images = job.Images,
				CompanyId = job.CompanyId,
				CategoryId = job.CategoryId,


			};
			if (job is null) throw new Exception("Not found");

			List<Job> relatedJobList = await _repository.GetAll(includes: new string[] { nameof(Job.Images) })
			   .Where(p => p.CategoryId == job.CategoryId && p.Id != id)
			   .ToListAsync();


			List<Job> companyjoblist = await _repository.GetAll(includes: new string[] { nameof(Job.Images) })
			   .Where(p => p.CompanyId == job.CompanyId && p.Id != id)
			   .ToListAsync();

			JobDetailVM detailVM = new JobDetailVM
			{
				RelatedJobs = relatedJobList,
				CompanyJobs = companyjoblist,
				Job = jobItemVM
			};
			return detailVM;
		}

		//public async Task<bool> UpdateAsync(UpdateJobVM JobVm, ModelStateDictionary modelState, int id)
		//{
		//	if (!modelState.IsValid) return false;
		//	Job job = await _repository.GetByIdAsync(id, isDeleted: false);
		//	if (job == null) throw new Exception("Not found");
		//	if (await _company.IsExist(x => x.Id == JobVm.CompanyId) == false)
		//	{
		//		modelState.AddModelError("CompanyId", "You dont have this Company");
		//		return false;
		//	}
		//	if (JobVm.CategoryId is not null)
		//	{
		//		if (!await _company.IsExist(x => x.Id == JobVm.CategoryId))
		//		{
		//			modelState.AddModelError("CategoryId", "You dont have this Category");
		//			return false;
		//		}

		//	}
		//	if (JobVm.Name != job.Name)
		//	{
		//		ICollection<Job> Jobs = await _repository.GetAllnotDeleted().ToListAsync();

		//		if (Jobs.Where(x => x.Name == JobVm.Name && x.CompanyId == JobVm.CompanyId).Count() >= 1)
		//		{
		//			modelState.AddModelError("Name", "You have this Job in your Jobs");
		//			return false;
		//		}

		//	}
		//	job.Name = JobVm.Name;
		//	job.JobNature = JobVm.JobNature;
		//	job.Experience = JobVm.Experience;
		//	job.Vacancy = JobVm.Vacancy;
		//	job.Requirement = JobVm.Requirement;
		//	job.ModifiedAt = DateTime.UtcNow;
		//	job.Deadline = (DateTime)JobVm.Deadline;
		//	job.Salary = (decimal)JobVm.Salary;
		//	job.CategoryId = JobVm.CategoryId;


		//	if (JobVm.Photo != null)
		//	{
		//		if (!JobVm.Photo.ValidateType("image/"))
		//		{
		//			modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
		//			return false;
		//		}
		//		if (!JobVm.Photo.ValidateSize(5 * 1024))
		//		{
		//			modelState.AddModelError("Photo", "Your photo size max be 5mb");
		//			return false;
		//		}


		//		if (JobVm.Photo != null)
		//		{
		//			string fileName = await JobVm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon");
		//			Image main = job.Images.FirstOrDefault(pi => pi.IsPrimary == true);
		//			main.Url.DeleteFile(_env.WebRootPath, "assets", "images", "icon");
		//			job.Images.Remove(main);

		//			job.Images.Add(new Image
		//			{
		//				IsPrimary = true,
		//				Url = fileName
		//			});
		//		}



		//	}
		//	_repository.Update(job);
		//	await _repository.SaveChangesAsync();
		//	return true;
		//}

		//public async Task<bool> UpdateAsync(UpdateJobVM UpdateJobVM, ModelStateDictionary modelState, int id)
		//{
		//	if (!modelState.IsValid) return false;
		//	Job existed = await _repository.GetByIdAsync(id, isDeleted: false, includes: new string[] { "Images", "CompanyCities", "CompanyCities.City" });
		//	if (existed == null) throw new Exception("Not found");
		//	if (UpdateJobVM.Name != existed.Name)
		//		if (await _repository.IsExistAsync(x => x.Name == UpdateJobVM.Name))
		//		{
		//			modelState.AddModelError("Name", "You have same name company like this, please change name");
		//			return false;
		//		}

		//	existed.Name = UpdateJobVM.Name;
		//	existed.IsDeleted = null;
		//	existed.Requirement = UpdateJobVM.Requirement;
		//	existed.Experience = UpdateJobVM.Experience;
		//	existed.Vacancy = UpdateJobVM.Vacancy;
		//	existed.Deadline = UpdateJobVM.Deadline;
		//	existed.CategoryId = UpdateJobVM.CategoryId;
		//	existed.JobNature = UpdateJobVM.JobNature;
		//	existed.Salary = UpdateJobVM.Salary;
		//	existed.d = UpdateJobVM.Requirement;
		//	existed.Requirement = UpdateJobVM.Requirement;








		//	_repository.Update(existed);



		//	if (UpdateJobVM.CityIds != null)
		//	{
		//		var existingCityIds = existed.CompanyCities.Select(cc => cc.CityId).ToList();
		//		var citiesToRemove = existingCityIds.Except(UpdateJobVM.CityIds).ToList();

		//		foreach (var cityId in citiesToRemove)
		//		{
		//			var cityToRemove = existed.CompanyCities.FirstOrDefault(cc => cc.CityId == cityId);
		//			if (cityToRemove != null)
		//			{
		//				existed.CompanyCities.Remove(cityToRemove);
		//			}
		//		}
		//	}


		//	if (UpdateJobVM.CityIds != null)
		//	{
		//		foreach (var cityId in UpdateJobVM.CityIds)
		//		{
		//			if (!existed.CompanyCities.Any(cc => cc.CityId == cityId))
		//			{
		//				existed.CompanyCities.Add(new CompanyCity { CityId = cityId });
		//			}
		//		}
		//	}




		//	if (UpdateJobVM.Photo != null)
		//	{
		//		Image main = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
		//		if (main != null && main.Id > 0)
		//		{
		//			main.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
		//			existed.Images.Remove(main);
		//		}


		//		string fileName = await UpdateJobVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon");
		//		existed.Images.Add(new Image
		//		{
		//			IsPrimary = true,
		//			Url = fileName
		//		});
		//	}

		//	await _repository.SaveChangesAsync();
		//	return true;


		//}





		public async Task<UpdateJobVM> UpdatedAsync(int id)
		{
			if (id < 1) throw new Exception("Bad Request");

			Job existed = await _repository.GetByIdAsync(id, isDeleted: false, includes: new string[] { "Images" });
			if (existed == null) throw new Exception("Not Found");

			return new UpdateJobVM
			{
				Requirement = existed.Requirement,
				CategoryId=existed.CategoryId,
				Salary=existed.Salary,
				JobNature=existed.JobNature,
				Vacancy=existed.Vacancy,
				Experience=existed.Experience,
				Deadline=existed.Deadline,
				CompanyId=existed.CompanyId,
				Name = existed.Name,
				
			};
		}
		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception("Bad Request");
			Job existed = await _repository.GetByIdAsync(id);
			if (existed == null) throw new Exception("Not Found");
			existed.Images.FirstOrDefault(i => i.IsPrimary == true).Url.DeleteFile(_env.WebRootPath, "assets", "img", "JobImages");
			_repository.Delete(existed);
			await _repository.SaveChangesAsync();
		}


		public async Task ReverseDeleteAsync(int id)
		{
			if (id < 1) throw new Exception("Bad Request");
			Job existed = await _repository.GetByIdAsync(id, isDeleted: true);
			if (existed == null) throw new Exception("Not Found");
			_repository.ReverseDelete(existed);
			await _repository.SaveChangesAsync();
		}
		public async Task Submit(int id)
		{
			if (id < 1) throw new Exception("Bad Request");
			Job existed = await _repository.GetByIdAsync(id, isDeleted: null);
			if (existed == null) throw new Exception("Not Found");
			_repository.ReverseDelete(existed);
			await _repository.SaveChangesAsync();
		}

		public async Task SoftDeleteAsync(int id)
		{
			if (id < 1) throw new Exception("Bad Request");
			Job existed = await _repository.GetByIdAsync(id, isDeleted: false);
			if (existed == null) throw new Exception("Not Found");
			_repository.SoftDelete(existed);
			await _repository.SaveChangesAsync();
		}
	}



}


