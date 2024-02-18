using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProjectFb.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }
		public async Task<IActionResult> Index(int page = 1, int take = 10)
		{

			
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				
				return RedirectToAction("Login", "Account"); 
			}
            if (!User.IsInRole("Creater"))
            {
                return RedirectToAction("Login", "Account");
            }


            PaginateVM<Company> vm = await _service.GetCompaniesCreatedByUserAsync(userId, page, take);

			if (vm.Items == null)
				return NotFound();

			return View(vm);
		}

		public async Task<IActionResult> ConfirmationForm()
        {
            ConfirmationFormVM confirmationFormVM = new ConfirmationFormVM();
            confirmationFormVM.CityIds = new List<int>(); 
            confirmationFormVM = await _service.GetCitiesForConfirmationFormAsync(confirmationFormVM);
            return View(confirmationFormVM);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmationForm(ConfirmationFormVM confirmationFormVM)
        {
            if (await _service.GetConfirmationFormAsync(confirmationFormVM, ModelState))
                return RedirectToAction("Index","Home");
            return View(await _service.GetCitiesForConfirmationFormAsync(confirmationFormVM));
        }


        public async Task<IActionResult> Detail(int id)
        {
            return View(await _service.DetailAsync(id));
        }


        public async Task<IActionResult> Update(int id)
        {
            return View(await _service.UpdatedAsync( id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateCompanyVM updateVm)
        {
            if (await _service.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync( id));
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return RedirectToAction("Index"); // Başarılıysa bir başka sayfaya yönlendirme yapabilirsiniz.
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir işlem yapabilirsiniz, örneğin hata mesajını loglama.
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");

            }
            
        }
    }
}
