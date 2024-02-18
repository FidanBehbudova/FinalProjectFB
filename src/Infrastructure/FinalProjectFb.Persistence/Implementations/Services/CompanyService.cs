using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Enums;
using FinalProjectFb.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly ICityRepository _cityRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _user;
        private readonly IJobRepository _jobRepository;

        public CompanyService(ICompanyRepository repository, ICityRepository cityRepository, IWebHostEnvironment env, IHttpContextAccessor accessor, IUserService user,IJobRepository jobRepository)
        {
            _repository = repository;
            _cityRepository = cityRepository;
            _env = env;
            _accessor = accessor;
            _user = user;
            _jobRepository = jobRepository;
        }
        public async Task<PaginateVM<Company>> GetCompaniesCreatedByUserAsync(string userId, int page = 1, int take = 10)
        {

            ICollection<Company> companies = await _repository.GetPaginationC<Company>(c => c.AppUserId == userId, skip: (page - 1) * take, take: take).ToListAsync();


            int count = await _repository.GetAll().CountAsync(c => c.AppUserId == userId);
            double totalpage = Math.Ceiling((double)count / take);

            PaginateVM<Company> vm = new PaginateVM<Company>
            {
                Items = companies,
                CurrentPage = page,
                TotalPage = totalpage
            };

            return vm;
        }
        public async Task<bool> GetConfirmationFormAsync(ConfirmationFormVM vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;

            if (await _repository.IsExistAsync(c => c.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This Company already exists");
                return false;
            }

            AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);

            Image photo = new Image
            {
                IsPrimary = true,
                Url = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon")
            };

            Company company = new Company
            {
                AppUserId = User.Id,
                CreatedBy = User.UserName,
                CreatedAt = DateTime.UtcNow,
                Name = vm.Name,
                WebsiteLink = vm.WebsiteLink,
                FacebookLink = vm.FacebookLink,
                TwitterLink = vm.TwitterLink,
                GmailLink = vm.GmailLink,
                InstagramLink = vm.InstagramLink,
                IsDeleted = null,
                CompanyCities = new List<CompanyCity>(),
                Description = vm.Description,
                Images = new List<Image> { photo },
            };

            if (vm.CityIds != null)
            {
                foreach (var item in vm.CityIds)
                {
                    if (!await _cityRepository.IsExistAsync(c => c.Id == item))
                    {
                        modelstate.AddModelError(String.Empty, "This city does not exist");
                        return false;
                    }
                    company.CompanyCities.Add(new CompanyCity
                    {
                        CityId = item
                    });
                }
            }

            await _repository.AddAsync(company);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Company existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Company existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task<ConfirmationFormVM> GetCitiesForConfirmationFormAsync(ConfirmationFormVM confirmationFormVM)
        {
            try
            {
                var cityList = _cityRepository.GetAll().ToList();
                confirmationFormVM.Cities = cityList;
                return confirmationFormVM;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in GetCitiesForConfirmationFormAsync: {ex.Message}");
                return confirmationFormVM;
            }
        }
        public async Task<UpdateCompanyVM> GetCitiesForUpdateFormAsy(UpdateCompanyVM updateCompanyVM)
        {
            try
            {
                var cityList = _cityRepository.GetAll().ToList();
                updateCompanyVM.Cities = cityList;
                return updateCompanyVM;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in GetCitiesForConfirmationFormAsync: {ex.Message}");
                return updateCompanyVM;
            }
        }


        public async Task<PaginateVM<Company>> GetAllAsync(int page = 1, int take = 10)
        {
            ICollection<Company> companies = await _repository.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();
            int count = await _repository.GetAll().CountAsync();
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<Company> vm = new PaginateVM<Company>
            {
                Items = companies,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        public async Task<CompanyDetailVM> DetailAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException("id");

            Company company = await _repository.GetByIdAsync(id, includes: new string[] { "CompanyCities", "Images" });


            CompanyItemVM companyItemVM = new CompanyItemVM
            {

                Name = company.Name,
                Images = company.Images,
                FacebookLink = company.FacebookLink,
                TwitterLink = company.TwitterLink,
                InstagramLink = company.InstagramLink,
                GmailLink = company.GmailLink,
                WebsiteLink = company.WebsiteLink,
                Description = company.Description,
                CompanyCities = company.CompanyCities,
                CompanyId = company.Id
            };
            if (company is null) throw new Exception("Not found");

            CompanyDetailVM detailVM = new CompanyDetailVM
            {

                Company = companyItemVM
            };
            return detailVM;
        }

        public async Task<bool> UpdateAsync(UpdateCompanyVM updateCompanyVM, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            Company existed = await _repository.GetByIdAsync(id, isDeleted: false,includes:new string[] { "Images", "CompanyCities", "CompanyCities.City" });
            if (existed == null) throw new Exception("Not found");
            if (updateCompanyVM.Name != existed.Name)
                if (await _repository.IsExistAsync(x => x.Name == updateCompanyVM.Name))
                {
                    modelState.AddModelError("Name", "You have same name company like this, please change name");
                    return false;
                }

            existed.Name = updateCompanyVM.Name;
            existed.IsDeleted = null;
            existed.FacebookLink = updateCompanyVM.FacebookLink;
            existed.TwitterLink = updateCompanyVM.TwitterLink;
            existed.GmailLink = updateCompanyVM.GmailLink;
            existed.InstagramLink = updateCompanyVM.InstagramLink;
            existed.Description = updateCompanyVM.Description;         
            existed.WebsiteLink = updateCompanyVM.WebsiteLink;


            _repository.Update(existed);
          

   
            if (updateCompanyVM.CityIds != null)
            {
                var existingCityIds = existed.CompanyCities.Select(cc => cc.CityId).ToList();
                var citiesToRemove = existingCityIds.Except(updateCompanyVM.CityIds).ToList();

                foreach (var cityId in citiesToRemove)
                {
                    var cityToRemove = existed.CompanyCities.FirstOrDefault(cc => cc.CityId == cityId);
                    if (cityToRemove != null)
                    {
                        existed.CompanyCities.Remove(cityToRemove);
                    }
                }
            }

          
            if (updateCompanyVM.CityIds != null)
            {
                foreach (var cityId in updateCompanyVM.CityIds)
                {
                    if (!existed.CompanyCities.Any(cc => cc.CityId == cityId))
                    {
                        existed.CompanyCities.Add(new CompanyCity { CityId = cityId });
                    }
                }
            }

           


            if (updateCompanyVM.Photo != null)
            {
                Image main = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                if (main != null && main.Id > 0)
                {
                    main.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
                    existed.Images.Remove(main);
                }

              
                string fileName = await updateCompanyVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "icon");
                existed.Images.Add(new Image
                {
                    IsPrimary = true,
                    Url = fileName
                });
            }

            await _repository.SaveChangesAsync();
            return true;

            
        }


        public async Task<UpdateCompanyVM> UpdatedAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");

            Company existed = await _repository.GetByIdAsync(id, isDeleted: false, includes: new string[] { "Images", "CompanyCities", "CompanyCities.City" });
            if (existed == null) throw new Exception("Not Found");
            
            return new UpdateCompanyVM
            {
                TwitterLink = existed.TwitterLink,
                Name = existed.Name,
                WebsiteLink = existed.WebsiteLink,
                FacebookLink = existed.FacebookLink,
                InstagramLink = existed.InstagramLink,
                GmailLink = existed.GmailLink,
                Description = existed.Description,
                Images= existed.Images,
                CityIds=existed.CompanyCities.Select(c => c.CityId).ToList(),
               Cities=await _cityRepository.GetAll().ToListAsync(),
            };
        }

        //public async Task DeleteAsync(int id)
        //{
        //    if (id < 1) throw new Exception("Bad Request");
        //    Company existed = await _repository.GetByIdnotDeletedAsync(id, includes: new string[] { "Images", "CompanyCities", "CompanyCities.City", "Jobs" });
        //    if (existed == null) throw new Exception("Not Found");

        //    // Şirkete ait tüm resimleri sil
        //    foreach (var image in existed.Images.ToList() ?? new List<Image>())
        //    {
        //        image.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
        //    }

        //    // Şirkete ait tüm jobları sil
        //    if (existed.Jobs is not null)
        //    {
        //        foreach (var job in existed.Jobs.ToList() ?? new List<Job>())
        //        {
        //            // Job'a ait tüm resimleri sil
        //            if (job.Images != null)
        //            {
        //                foreach (var image in job.Images.ToList())
        //                {
        //                    image.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
        //                }
        //            }

        //            // Job'u sil
        //            _jobRepository.Delete(job);
        //        }

        //        // Şirketin Jobs koleksiyonunu temizle
        //        existed.Jobs.Clear();
        //    }

        //    // Şirketi sil
        //    _repository.Delete(existed);
        //    await _repository.SaveChangesAsync();
        //}

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Company existed = await _repository.GetByIdnotDeletedAsync(id, includes: new string[] { "Images", "CompanyCities", "CompanyCities.City","Jobs" });
            if (existed == null) throw new Exception("Not Found");

            // Şirkete ait tüm resimleri sil
            foreach (var image in existed.Images.ToList() ?? new List<Image>())
            {
                image.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
            }


            if (existed.Jobs is not null)
            {
                foreach (var job in existed.Jobs.ToList() ?? new List<Job>())
                {

                    if (job.Images != null)
                    {
                        foreach (var image in job.Images.ToList())
                        {
                            image.Url.DeleteFile(_env.WebRootPath, "assets", "img", "icon");
                        }
                    }


                    _jobRepository.Delete(job);
                }


                existed.Jobs.Clear();
            }

            // Şirketi sil
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }

        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id <= 0) return BadRequest();
        //    Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

        //    if (product == null) return NotFound();
        //    foreach (var item in product.ProductImages ?? new List<ProductImage>())
        //    {
        //        item.Url.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


    }
}
