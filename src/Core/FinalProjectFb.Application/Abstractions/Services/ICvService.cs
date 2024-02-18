using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels.Cv;
using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
	public interface ICvService
	{
		Task<bool> CreateAsync(CreateCvVM vm, ModelStateDictionary modelstate);
		Task<PaginateVM<Cv>> GetAllAsync(int id, int page = 1, int take = 5);

    }
}
