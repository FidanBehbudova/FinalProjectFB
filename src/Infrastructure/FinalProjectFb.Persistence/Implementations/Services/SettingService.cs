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

public class SettingService : ISettingService
{
    private readonly ISettingRepository _repository;
    private readonly IUserService _user;
    private readonly IHttpContextAccessor _accessor;

    public SettingService(ISettingRepository repository, IUserService user, IHttpContextAccessor accessor)
    {
        _repository = repository;
        _user = user;
        _accessor = accessor;
    }
    public async Task<bool> CreateAsync(CreateSettingVM vm, ModelStateDictionary modelstate)
    {
        if (!modelstate.IsValid) return false;

        if (await _repository.IsExistAsync(c => c.Key == vm.Key))
        {
            modelstate.AddModelError("Key", "This Setting is already exist");
            return false;
        }
        AppUser User = await _user.GetUser(_accessor.HttpContext.User.Identity.Name);
        await _repository.AddAsync(new Setting
        {
            CreatedBy = User.UserName,
            Key = vm.Key,
            Value = vm.Value,

        });
        await _repository.SaveChangesAsync();
        return true;
    }



    public async Task<PaginateVM<Setting>> GetAllAsync(int page = 1, int take = 10)
    {
        ICollection<Setting> categories = await _repository.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();
        int count = await _repository.GetAll().CountAsync();
        double totalpage = Math.Ceiling((double)count / take);
        PaginateVM<Setting> vm = new PaginateVM<Setting>
        {
            Items = categories,
            CurrentPage = page,
            TotalPage = totalpage
        };
        return vm;
    }



    public async Task<bool> UpdateAsync(int id, UpdateSettingVM vm, ModelStateDictionary modelstate)
    {
        if (id < 1) throw new ArgumentOutOfRangeException("id");
        if (!modelstate.IsValid) return false;

        Setting exist = await _repository.GetByIdAsync(id);
        if (exist == null) throw new Exception("Not found");


        if (await _repository.IsExist(l => l.Key == vm.Key && l.Id != id))
        {
            modelstate.AddModelError("Key", "This Setting is already exist");
            return false;
        }

        exist.Key = vm.Key.Trim();
        exist.Value = vm.Value;

        _repository.Update(exist);
        await _repository.SaveChangesAsync();
        return true;
    }




    public async Task<UpdateSettingVM> UpdatedAsync(int id, UpdateSettingVM vm)
    {
        if (id < 1) throw new ArgumentOutOfRangeException("id");
        Setting exist = await _repository.GetByIdAsync(id);
        if (exist == null) throw new Exception("Not found");

        vm.Key = exist.Key.Trim();
        vm.Value = exist.Value;
        return vm;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id < 1) throw new ArgumentOutOfRangeException("id");
        Setting exist = await _repository.GetByIdAsync(id);
        if (exist == null) throw new Exception("Not found");
        _repository.Delete(exist);
        await _repository.SaveChangesAsync();
        return true;
    }


}
