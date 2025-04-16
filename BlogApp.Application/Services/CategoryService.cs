using AutoMapper;
using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
    public class CategoryService : ICategoryService, ICategoryValidateService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _categoryCreateValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateValidator;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper,IValidator<CategoryCreateDto> categoryCreateValidator, IValidator<CategoryUpdateDto> categoryUpdateValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryCreateValidator = categoryCreateValidator;
            _categoryUpdateValidator = categoryUpdateValidator;
        }

        public async Task AddCategoryAsync(CategoryCreateDto category)
        {
            await _categoryRepository.AddAsync(new Category { Name = category.Name });
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper?.Map<List<CategoryDto>>(categories) ?? categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
        }

        public async Task UpdateCategoryAsync(CategoryUpdateDto category)
        {
            await _categoryRepository.UpdateAsync(new Category { Id = category.Id, Name = category.Name });
        }

        public async Task<ValidationResult> ValidateAddCategoryAsync(CategoryCreateDto model)
        {
            return await _categoryCreateValidator.ValidateAsync(model);
        }

        public async Task<ValidationResult> ValidateUpdateCategoryAsync(CategoryUpdateDto model)
        {
            return await _categoryUpdateValidator.ValidateAsync(model);
        }
    }
}
