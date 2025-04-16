using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserRegistrationModel> _registrationValidator;
        private readonly IValidator<UserLoginModel> _loginValidator;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserService(IUserRepository userRepository, IValidator<UserRegistrationModel> registrationValidator, IPasswordHasherService passwordHasherService, IValidator<UserLoginModel> loginValidator)
        {
            _userRepository = userRepository;
            _registrationValidator = registrationValidator;
            _passwordHasherService = passwordHasherService;
            _loginValidator = loginValidator;
        }

        public async Task<ValidationResult> ValidateRegistrationAsync(UserRegistrationModel model)
        {
            return await _registrationValidator.ValidateAsync(model);
        }

        public async Task<bool> IsUserNameUniqueAsync(string userName)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(userName);
            return existingUser == null;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationModel model)
        {
            var (passwordHash, passwordSalt) = _passwordHasherService.HashPassword(model.Password);

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserTypeId = (int?)UserRole.Normal,
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }

        public async Task<bool> VerifyUserPasswordAsync(string userName, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user == null)
            {
                return false;
            }
            return _passwordHasherService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }

        public async Task<ValidationResult> ValidateLoginAsync(UserLoginModel model)
        {
            return await _loginValidator.ValidateAsync(model);
        }

        public async Task<User> AuthenticateUserAsync(UserLoginModel model)
        {
            var user = await _userRepository.GetByUsernameAsync(model.UserName);
            if (user == null)
            {
                return null;
            }

            if (_passwordHasherService.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return user;
            }

            return null;
        }
    }
}
