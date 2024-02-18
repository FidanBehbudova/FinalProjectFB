using AutoMapper;
using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities;
using FinalProjectFb.Application.ViewModels.Users;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, IMapper mapper,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<string>> Register(RegisterVM vm)
        {
            List<string> str = new List<string>();
            if (!vm.Name.IsLetter())
            {
                str.Add("Your Name or Surname only contain letters");
                return str;
            }
            if (!vm.Email.CheeckEmail())
            {
                str.Add("Your Email type is not true");
                return str;
            }
            vm.Name.Capitalize();
            vm.Surname.Capitalize();
            if (!vm.Gender.CheeckGender())
            {
                str.Add("Your Gender is not exis");
                return str;
            }

            AppUser user = new AppUser
            {
                Name = vm.Name,
                UserName = vm.Username,
                Surname = vm.Surname,
                Email = vm.Email,
                Birthday = vm.Birthday,
                Gender = vm.Gender
            };
            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {

                    str.Add(error.Description);
                }
                return str;
            }
            if (user != null)
            {
                await AssignRoleToUser(user, vm.Role.ToString());
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return str;

        }
        public async Task AssignRoleToUser(AppUser user, string roleName)
        {
           
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }           
            await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<List<string>> Login(LoginVM vm)
        {
            List<string> str = new List<string>();
            AppUser user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    str.Add("Username, Email or Password was wrong");
                    return str;

                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if (result.IsLockedOut)
            {
                str.Add("You have a lot of fail  try that is why you banned please try some minuts late");
                return str;
            }
            if (!result.Succeeded)
            {
                str.Add("Username, Email or Password was wrong");
                return str;
            }
            
            return str;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task CreateRoleAsync()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });

                }
            }
        }
        public async Task CreateAdminRoleAsync()
        {
            var adminRoleName = "Admin";

            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = adminRoleName,
                });
            }
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await _userManager.Users

                .FirstOrDefaultAsync(u => u.Id == userId);


        }
        public async Task<AppUser> GetUser(string username)
        {
            return await _userManager.Users
                
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<ICollection<AppUser>> GetAllUsers(string searchTerm)
        {
            return await _userManager.Users.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower()) || x.Name.ToLower().Contains(searchTerm.ToLower()) || x.Surname.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }
        public async Task UpdateUserRole(string userId, string roleName)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User Not found");
            }
            IList<string> existingRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in existingRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.UpdateAsync(user);
        }
        
        public async Task<List<string>> UpdateUser(string userId, UpdateUserVM vm)
        {
            List<string> str = new List<string>();

           
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                str.Add("User not found");
                return str;
            }

          
            if (!string.IsNullOrEmpty(vm.Name) && vm.Name.IsLetter())
            {
                user.Name = vm.Name.Capitalize();
            }

            if (!string.IsNullOrEmpty(vm.Email) && vm.Email.CheeckEmail())
            {
                user.Email = vm.Email;
            }

            if (!string.IsNullOrEmpty(vm.Password))
            {
                IdentityResult passwordResult = await _userManager.ChangePasswordAsync(user, null, vm.Password);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        str.Add(error.Description);
                    }
                }
            }

            if (!string.IsNullOrEmpty(vm.Username))
            {
                user.UserName = vm.Username;
            }

            if (!string.IsNullOrEmpty(vm.Surname) && vm.Surname.IsLetter())
            {
                user.Surname = vm.Surname.Capitalize();
            }

            if (vm.Gender.HasValue)
            {
                user.Gender = vm.Gender.Value;
            }

            if (vm.Birthday.HasValue)
            {
                user.Birthday = vm.Birthday.Value;
            }

            

            if (vm.Role.HasValue)
            {
                await UpdateUserRole(userId, vm.Role.ToString());
            }

          
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    str.Add(error.Description);
                }
            }

            return str;
        }

    }
}
