using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using BlogApp.Web.Filters;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegistrationModel model)
        {
            var validationResult = await _userService.ValidateRegistrationAsync(model);
            ModelState.Clear();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            if (!await _userService.IsUserNameUniqueAsync(model.UserName))
            {
                ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten alınmış.");
                return View(model);
            }

            var registeredUser = await _userService.RegisterUserAsync(model);

            if (registeredUser != null)
            {
                TempData["SuccessMessage"] = "Kayıt işleminiz başarılı bir şekilde gerçekleşti.";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("", "Kayıt işlemi başarısız oldu.");
                return View(model); 
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var validationResult = await _userService.ValidateLoginAsync(model);
            ModelState.Clear();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            User user = await _userService.AuthenticateUserAsync(model);
            if (user != null)
            {

                HttpContext.Response.Cookies.Append(
                    "UserSessionId",
                    user.Id.ToString(),
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30), HttpOnly = true, Secure = Request.IsHttps, SameSite = SameSiteMode.Strict }
                );
                HttpContext.Response.Cookies.Append(
                    "UserSessionRole",
                    user.UserTypeId.ToString(),
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30), HttpOnly = true, Secure = Request.IsHttps, SameSite = SameSiteMode.Strict }
                );
                if(user.UserTypeId == (int)UserRole.Admin)
                    return RedirectToAction("CategoryList", "Category");
                return RedirectToAction("BlogList", "Blog");
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            var userRole = Convert.ToInt32(HttpContext.Items["UserRole"] as string);
            HttpContext.Response.Cookies.Delete("UserSessionId");
            HttpContext.Response.Cookies.Delete("UserSessionRole");
            if (userRole == (int)UserRole.Admin)
                return RedirectToAction("Login", "Auth");
            return RedirectToAction("BlogList", "Blog");
        }
    }
}
