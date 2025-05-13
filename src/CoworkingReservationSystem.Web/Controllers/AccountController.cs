using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoworkingReservationSystem.Web.Models.ViewModels;
using CoworkingReservationSystem.Web.Models.Auth;
namespace CoworkingReservationSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.PostAsync<LoginResponse>("api/Auth/login", model);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError("", response.Error ?? "Credenciais inválidas");
                return View(model);
            }

            HttpContext.Session.SetString("JwtToken", response.Data.Token);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.PostAsync("api/Auth/register", model);
            if (response.IsSuccess)
                return RedirectToAction("Login");

            ModelState.AddModelError("", response.Error);
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}