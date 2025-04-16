using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Application.Services;
using BlogApp.Domain.Enums;
using BlogApp.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Web.Controllers
{
    [RoleAuthorize(UserRole.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryValidateService _categoryValidateService;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, ICategoryValidateService categoryValidateService)
        {
            _categoryService = categoryService;
            _categoryValidateService = categoryValidateService;
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto model)
        {
            var validationResult = await _categoryValidateService.ValidateAddCategoryAsync(model);
            if (!validationResult.IsValid)
            {
                TempData["Error"] = "Kategori adı gereklidir.";
                return RedirectToAction("CategoryList");
            }

            TempData["Success"] = "Categori Ekleme İşleminiz Başarılı Bir Şekilde Gerçekleşti";
            await _categoryService.AddCategoryAsync(model);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto model)
        {
            var validationResult = await _categoryValidateService.ValidateUpdateCategoryAsync(model);
            if (!validationResult.IsValid)
            {
                TempData["Error"] = "Kategori adı güncellenemedi, geçerli bir değer girin.";
                return RedirectToAction("CategoryList");
            }

            TempData["Success"] = "Categori Güncelleme İşleminiz Başarılı Bir Şekilde Gerçekleşti";
            await _categoryService.UpdateCategoryAsync(model);
            return RedirectToAction("CategoryList");
        }
    }
}
