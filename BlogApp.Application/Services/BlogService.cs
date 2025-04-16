using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using BlogApp.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
    public class BlogService : IBlogService, IBlogValidateService
    {
        private readonly IOpenAIService _openAIService;
        private readonly IBlogRepository _blogRepository;
        private readonly IValidator<BlogCreateDto> _blogCreateValidator;
        private readonly IValidator<BlogUpdateDto> _blogUpdateValidator;
        private readonly ICommentService _commentService;
        private readonly IImageService _imageService;

        public BlogService(IBlogRepository blogRepository, IValidator<BlogCreateDto> blogCreateValidator, IValidator<BlogUpdateDto> blogUpdateValidator, ICommentService commentService, IImageService imageService, IOpenAIService openAIService)
        {
            _openAIService = openAIService;
            _blogRepository = blogRepository;
            _blogCreateValidator = blogCreateValidator;
            _blogUpdateValidator = blogUpdateValidator;
            _commentService = commentService;
            _imageService = imageService;   

        }
        public async Task<BlogDto> AddBlogAsync(BlogCreateDto blogCreateDto, int userId, IFormFile imageFile)
        {
            if(imageFile != null)
            {
                string imageUrl = await _imageService.UploadImageAsync(imageFile);
                if(imageUrl is null) {
                    return null;
                }
                blogCreateDto.ImageUrl = imageUrl;
            }
            var newBlog = new Blog
            {
                Title = blogCreateDto.Title,
                Content = blogCreateDto.Content,
                PublishDate = DateTime.UtcNow,
                CategoryId = blogCreateDto.CategoryId,
                UserId = userId,
                ImageUrl = blogCreateDto.ImageUrl,
                Type = await _openAIService.IsContentAdultAsync(blogCreateDto.Content),
            };

            await _blogRepository.AddBlogAsync(newBlog);

            var created = new BlogDto
            {
                Id = newBlog.Id,
                Title = newBlog.Title,
                Content = newBlog.Content,
                ImageUrl = newBlog.ImageUrl,
                PublishDate = newBlog.PublishDate,
            };

            return created;
        }

        public async Task DeleteBlogAsync(int blogId)
        {
            await _blogRepository.DeleteBlogAsync(blogId);
        }

        public async Task<BlogDto> UpdateBlogAsync(BlogUpdateDto blogUpdateDto,IFormFile imageFile)
        {
            var blog = await _blogRepository.GetBlogById(blogUpdateDto.Id);
            if(!string.IsNullOrEmpty(blog.ImageUrl)) {
                if (!await _imageService.DeleteImageAsync(blog.ImageUrl))
                {
                    return null;
                }
                if (imageFile != null)
                {
                    string imageUrl = await _imageService.UploadImageAsync(imageFile);
                    if (imageUrl is null)
                    {
                        return null;
                    }
                    blogUpdateDto.ImageUrl = imageUrl;
                }
                else
                {
                    blogUpdateDto.ImageUrl= null;
                }
            }

            blog.Title = blogUpdateDto.Title;
            blog.Content = blogUpdateDto.Content;
            blog.ImageUrl = blogUpdateDto.ImageUrl;
            blog.CategoryId = blogUpdateDto.CategoryId;
            blog.Type = await _openAIService.IsContentAdultAsync(blogUpdateDto.Content);

            await _blogRepository.UpdateBlogAsync(blog);

            var updated = new BlogDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                PublishDate = blog.PublishDate,
            };

            return updated;
        }

        public async Task<BlogUpdateDto> GetBlogDataToUpdate(int blogId)
        {
            var blog = await _blogRepository.GetBlogById(blogId);
            return new BlogUpdateDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                CategoryId = blog.CategoryId,
                ImageUrl = blog.ImageUrl,
            };
        }

        public async Task<List<BlogModel>> GetBlogListInfoByAsync()
        {
            var blogListInfo = await _blogRepository.GetBlogListWithAuthorAndCategoryAsync();
            return blogListInfo.Select(b => new BlogModel 
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CategoryName = b.Category.Name,
                ImageUrl = b.ImageUrl,
                PublishDate = b.PublishDate,
                AuthorName = b.User.FirstName + " " + b.User.LastName,
                BlogType = b.Type,
            }).ToList();
        }

        public async Task<List<BlogModel>> GetBlogsByUserIdAsync(int userId)
        {
            var usersBlogs = await _blogRepository.GetBlogsByUserIdAsync(userId);
            return usersBlogs.Select(b => new BlogModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CategoryName = b.Category.Name,
                ImageUrl = b.ImageUrl,
                PublishDate = b.PublishDate,
                AuthorName = b.User.FirstName + " " + b.User.LastName,
            }).ToList();
        }

        public async Task<List<BlogModel>> GetBlogsByCategoryIdAsync(int categoryId)
        {
            var blogsfilterCategory = await _blogRepository.GetBlogsByCategoryIdAsync(categoryId);
            return blogsfilterCategory.Select(b => new BlogModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CategoryName = b.Category.Name,
                ImageUrl = b.ImageUrl,
                PublishDate = b.PublishDate,
                AuthorName = b.User.FirstName + " " + b.User.LastName,
                BlogType = b.Type,
            }).ToList();
        }

        public async Task<BlogModel> GetBlogInfoByIdAsync(int blogId)
        {
            var blogInfo = await _blogRepository.GetBlogbyIdWithAuthorAndCategoryAsync(blogId);
            return new BlogModel
            {
                Id = blogInfo.Id,
                Title = blogInfo.Title,
                Content = blogInfo.Content,
                CategoryName = blogInfo.Category.Name,
                ImageUrl = blogInfo.ImageUrl,
                PublishDate = blogInfo.PublishDate,
                AuthorName = blogInfo.User.FirstName + " " + blogInfo.User.LastName,
                BlogType= blogInfo.Type,
            };
        }

        public async Task<ValidationResult> ValidateAddBlogAsync(BlogCreateDto model)
        {
            return await _blogCreateValidator.ValidateAsync(model);
        }

        public async Task<ValidationResult> ValidateUpdateBlogAsync(BlogUpdateDto model)
        {
            return await _blogUpdateValidator.ValidateAsync(model);
        }

        public async Task<BlogDetailDto> GetBlogDetailsInfoByIdAsync(int blogId)
        {
            return new BlogDetailDto
            {
                Blog = await GetBlogInfoByIdAsync(blogId),
                Comments = await _commentService.GetCommentModelsByBlogId(blogId),
            };
        }
    }
}
