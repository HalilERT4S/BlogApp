using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ValidationResult> ValidateRegistrationAsync(UserRegistrationModel model);
        Task<bool> IsUserNameUniqueAsync(string userName);
        Task<User> RegisterUserAsync(UserRegistrationModel model);
        Task<ValidationResult> ValidateLoginAsync(UserLoginModel model);
        Task<User> AuthenticateUserAsync(UserLoginModel model);
    }
}
