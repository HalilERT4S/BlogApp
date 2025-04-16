using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Application.Services;
using BlogApp.Domain.Enums;
using BlogApp.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Web.Controllers
{
    [RoleAuthorize(UserRole.Guest, UserRole.Normal)]
    public class BlogController : Controller
    {
        private readonly IBlogValidateService _blogValidateService;
        private readonly IBlogService _blogService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _environment;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;

        public BlogController(IBlogService blogService, IWebHostEnvironment environment, IImageService imageService, ICategoryService categoryService, ICommentService commentService, IBlogValidateService blogValidateService)
        {
            _blogService = blogService;
            _environment = environment;
            _imageService = imageService;
            _categoryService = categoryService;
            _commentService = commentService;
            _blogValidateService = blogValidateService;
        }
        [NonAction]
        private async Task LoadCategoriesToViewBag()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateBlog()
        {
            await LoadCategoriesToViewBag();
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateBlog(BlogCreateDto blogCreateDto, IFormFile ImageFile)
        {
            var validationResult = await _blogValidateService.ValidateAddBlogAsync(blogCreateDto);
            ModelState.Clear();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                await LoadCategoriesToViewBag();
                return View(blogCreateDto);
            }
            var userId = Convert.ToInt32(HttpContext.Items["UserId"] as string);
            var addedBlog = await _blogService.AddBlogAsync(blogCreateDto, userId, ImageFile);

            if (addedBlog != null)
            {
                return RedirectToAction("MyBlogs", "Blog");
            }
            else
            {
                ViewBag.ErrorMessage = "Blog eklenirken bir hata oluştu.";
                await LoadCategoriesToViewBag();
                return View(blogCreateDto);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> UpdateBlog(int Id)
        {
            await LoadCategoriesToViewBag();
            var blogBeingEdited = await _blogService.GetBlogDataToUpdate(Id);
            return View(blogBeingEdited);
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> UpdateBlog(BlogUpdateDto blogUpdateDto, IFormFile imageFile)
        {
            var validationResult = await _blogValidateService.ValidateUpdateBlogAsync(blogUpdateDto);
            ModelState.Clear();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                await LoadCategoriesToViewBag();
                return View(blogUpdateDto);
            }

            var updatedBlog = await _blogService.UpdateBlogAsync(blogUpdateDto,imageFile);
            if (updatedBlog != null)
            {
                return RedirectToAction("MyBlogs", "Blog");
            }
            else
            {
                ViewBag.ErrorMessage = "Blog güncellenirken bir hata oluştu.";
                await LoadCategoriesToViewBag();
                return View(blogUpdateDto);
            }
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> MyBlogs()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"] as string);
            var blogs = await _blogService.GetBlogsByUserIdAsync(userId);
            return View(blogs);
        }


        public async Task<IActionResult> BlogList(int? categoryId)
        {
            List<BlogModel> blogs;
            if (categoryId.HasValue)
            {
                blogs = await _blogService.GetBlogsByCategoryIdAsync(categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }
            else
            {
                blogs = await _blogService.GetBlogListInfoByAsync();
                ViewBag.SelectedCategoryId = null;
            }

            await LoadCategoriesToViewBag();
            return View(blogs);
        }

        public async Task<IActionResult> BlogDetails(int Id)
        {
            var blogDetail = await _blogService.GetBlogDetailsInfoByIdAsync(Id);
            return View(blogDetail);
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> DeleteBlog(int Id)
        {
            await _blogService.DeleteBlogAsync(Id);
            return RedirectToAction("MyBlogs");
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> AddComment(int Id, string CommentText)
        {
            if (string.IsNullOrEmpty(CommentText))
            {
                TempData["ErrorMessage"] = "Yorum alanı boş olamaz.";
                return RedirectToAction("BlogDetails", new { Id = Id });
            }
            var userId = Convert.ToInt32(HttpContext.Items["UserId"] as string);
            if (!await _commentService.AddCommentAsync(new CommentAddModel { BlogId = Id, Text = CommentText, UserId = userId }))
            {
                TempData["ErrorMessage"] = "Yorum Oluşturulurken hata oluştu";
            }
            else
            {
                TempData["SuccessMessage"] = "Yorum Başarılı Bir Şekilde Oluşturuldu";
            }

            return RedirectToAction("BlogDetails", new { Id = Id });
        }
    }
}
