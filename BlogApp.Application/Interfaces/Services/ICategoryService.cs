using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task AddCategoryAsync(CategoryCreateDto category);
        Task UpdateCategoryAsync(CategoryUpdateDto category);
    }
}
