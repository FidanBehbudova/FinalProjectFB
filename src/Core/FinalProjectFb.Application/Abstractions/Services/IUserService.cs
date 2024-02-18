
using FinalProjectFb.Application.ViewModels.Users;
using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<List<string>> Register(RegisterVM vm);
        Task<List<string>> Login(LoginVM vm);
        Task Logout();
        Task CreateRoleAsync();
        Task CreateAdminRoleAsync();
        Task AssignRoleToUser(AppUser user, string roleName);
        Task<AppUser> GetUser(string username);
        Task<ICollection<AppUser>> GetAllUsers(string searchTerm);
        Task UpdateUserRole(string userId, string roleName);
        Task<List<string>> UpdateUser(string userId, UpdateUserVM vm);
        Task<AppUser> GetUserById(string userId);
    }
}
