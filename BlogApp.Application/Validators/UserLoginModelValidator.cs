using BlogApp.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Validators
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre alanı en az 6 karakter olmalıdır.");
        }
    }
}
