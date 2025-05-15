using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoworkingReservationSystem.Web.Models.ViewModels;
using CoworkingReservationSystem.Web.Models.Auth;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
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

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(response.Data.Token);

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, jwt.Subject),
            new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? model.Email),
            new Claim("access_token", response.Data.Token)                            
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties
        {
           // IsPersistent = vm.RememberMe,                       // se tiver “lembrar-me”
            ExpiresUtc = jwt.ValidTo                
        });

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
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}