using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoworkingReservationSystem.Web.Models.ViewModels;
namespace CoworkingReservationSystem.Web.Controllers
{
	public class RoomController : Controller
	{
		private readonly IApiService _apiService;

		public RoomController(IApiService apiService)
		{
			_apiService = apiService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
           // var endpoint = "api/Rooms";
          //  Console.WriteLine($"Chamando API: {_apiService.g}/{endpoint}");
            var response = await _apiService.GetAsync<List<RoomViewModel>>("api/Rooms");
			return View(response.Data);
		}
	}
}