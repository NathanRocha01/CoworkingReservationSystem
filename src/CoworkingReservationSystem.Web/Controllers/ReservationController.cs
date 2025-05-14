using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoworkingReservationSystem.Web.Models.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using static CoworkingReservationSystem.Web.Models.ViewModels.ReservationViewModel;
namespace CoworkingReservationSystem.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IApiService _apiService;

        public ReservationController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _apiService.GetAsync<List<ReservationViewModel>>("api/Reservations");

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return View(new List<ReservationViewModel>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _apiService.GetAsync<List<RoomViewModel>>("api/Rooms");

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Index");
            }

            var model = new ReservationViewModel
            {
                AvailableRooms = result.Data.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.Name} (Capacidade: {r.Capacity})"
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var roomsResult = await _apiService.GetAsync<List<RoomViewModel>>("api/Rooms");
                model.AvailableRooms = roomsResult.Data?.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList() ?? new List<SelectListItem>();

                return View(model);
            }

            var result = await _apiService.PostAsync<ReservationViewModel>("api/Reservations", model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);

                var roomsResult = await _apiService.GetAsync<List<RoomViewModel>>("api/Rooms");
                model.AvailableRooms = roomsResult.Data?.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList() ?? new List<SelectListItem>();

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPatch]
        public async Task<IActionResult> Cancel(int id)
        {
            var updateModel = new ReservationViewModel
            {
                Status = ReservationStatus.Canceled
            };

            var result = await _apiService.PatchAsync($"api/Reservations/{id}", updateModel);

            if (!result.IsSuccess)
                TempData["Error"] = result.Error;

            return RedirectToAction("Index");
        }
    }
}