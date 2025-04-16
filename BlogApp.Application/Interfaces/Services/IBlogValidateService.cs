using BlogApp.Application.Models;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface IBlogValidateService
    {
        Task<ValidationResult> ValidateAddBlogAsync(BlogCreateDto model);

        Task<ValidationResult> ValidateUpdateBlogAsync(BlogUpdateDto model);
    }
}
