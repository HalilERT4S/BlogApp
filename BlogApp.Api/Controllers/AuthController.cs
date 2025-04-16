using BlogApp.Web.Services;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public IActionResult Register()
        {
            return View(); // Kayıt formunu gösteren view'ı döndür
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            var validationResult = await _userService.ValidateRegistrationAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model); // Hatalarla birlikte kayıt formunu tekrar göster
            }

            if (!await _userService.IsUserNameUniqueAsync(model.UserName))
            {
                ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten alınmış.");
                return View(model); // Hata mesajıyla birlikte kayıt formunu tekrar göster
            }

            var registeredUser = await _userService.RegisterUserAsync(model);

            if (registeredUser != null)
            {
                // Kayıt başarılıysa kullanıcıyı giriş sayfasına yönlendirebilirsiniz
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("", "Kayıt işlemi başarısız oldu.");
                return View(model); // Hata mesajıyla birlikte kayıt formunu tekrar göster
            }
        }

        public IActionResult Login()
        {
            return View(); // Giriş formunu gösteren view'ı döndür
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var validationResult = await _userService.ValidateLoginAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model); // Hatalarla birlikte giriş formunu tekrar göster
            }

            User user = await _userService.AuthenticateUserAsync(model);
            if (user != null)
            {
                // Giriş başarılıysa, genellikle cookie tabanlı kimlik doğrulama yapılır
                // veya kullanıcı ana sayfaya yönlendirilir.
                // Token döndürmek yerine yönlendirme yapalım.
                // Not: Cookie tabanlı kimlik doğrulama için ek yapılandırma gerekebilir (Program.cs).
                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendir
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model); // Hata mesajıyla birlikte giriş formunu tekrar göster
            }
        }
    }
}
