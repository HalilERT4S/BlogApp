using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface IBlogService
    {
        Task<BlogDto> AddBlogAsync(BlogCreateDto blogCreateDto, int userId, IFormFile imageFile);

        Task<BlogDto> UpdateBlogAsync(BlogUpdateDto blogUpdateDto, IFormFile imageFile);

        Task<BlogUpdateDto> GetBlogDataToUpdate(int blogId);

        Task DeleteBlogAsync(int blogId);

        Task<BlogModel> GetBlogInfoByIdAsync(int blogId);

        Task<BlogDetailDto> GetBlogDetailsInfoByIdAsync(int blogId);

        Task<List<BlogModel>> GetBlogsByCategoryIdAsync(int categoryId);

        Task<List<BlogModel>> GetBlogListInfoByAsync();

        Task<List<BlogModel>> GetBlogsByUserIdAsync(int userId);
    }
}
