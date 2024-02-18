using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels.Users;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Enums;
using FinalProjectFb.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _email;

        public AccountController(IUserService service, UserManager<AppUser> userManager,IEmailService email)
        {
            _service = service;
            _userManager = userManager;
            _email = email;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _service.Register(vm);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                    return View(vm);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _service.Login(vm);

            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                }               
                return View(vm);
            }

            //if (User.IsInRole(UserRole.Creater.ToString()))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else if (User.IsInRole(UserRole.Member.ToString()))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    return RedirectToAction("Index", "manage");
            //}

            return RedirectToAction("Index", "Home");
           
        }



        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            await _service.CreateRoleAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateAdminRole()
        {
            await _service.CreateAdminRoleAsync();
            return RedirectToAction("Index", "Home");
        }
       
        public async Task<IActionResult> UpdateUser(string userId)
        {
            UpdateUserVM vm = new UpdateUserVM();
            await _service.UpdateUser(userId, vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(string userId, UpdateUserVM updateUserVM)
        {
            var result = await _service.UpdateUser(userId, updateUserVM);

            if (result.Any())
            {
                foreach (var error in result)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("Edit"); 
            }

           
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User Not Found");
                return View(vm);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
            await _email.SendEmailAsync(user.Email, "Reset Password", link, false);
            return RedirectToAction(nameof(Login));
        }


       


        public async Task<IActionResult> ResetPassword(string userid, string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm, string userid, string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, token, vm.ConfirmPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(vm);
            }
            return RedirectToAction(nameof(Login));
        }

    }

}

