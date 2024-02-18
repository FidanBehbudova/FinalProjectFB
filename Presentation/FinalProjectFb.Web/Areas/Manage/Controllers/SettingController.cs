using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;

using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly ISettingService _service;

        public SettingController(ISettingService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 10)
        {
            PaginateVM<Setting> vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            CreateSettingVM vm = new CreateSettingVM();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSettingVM vm)
        {
            if (await _service.CreateAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateSettingVM vm = new UpdateSettingVM();
            vm = await _service.UpdatedAsync(id, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVM vm)
        {
            if (await _service.UpdateAsync(id, vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(id, vm));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.DeleteAsync(id))
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

    }
}
