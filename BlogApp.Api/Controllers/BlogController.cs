using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] BlogCreateDto blogCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var newBlogDto = await _blogService.AddBlogAsync(blogCreateDto, userId);

            if (newBlogDto == null)
            {
                return BadRequest("Blog eklenirken bir hata oluştu.");
            }

            return Ok("");
        }

        public async Task<ActionResult<BlogDto>> UpdateBlog([FromBody] BlogUpdateDto blogUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateBlogDto = await _blogService.UpdateBlogAsync(blogUpdateDto);

            if (updateBlogDto == null)
            {
                return BadRequest("Blog güncellenirken bir hata oluştu.");
            }

            return Ok("");
        }

        public async Task<IActionResult> DeleteBlog([FromQuery] int blogId)
        {
            await _blogService.DeleteBlogAsync(blogId);
            return Ok();
        }
    }
}
