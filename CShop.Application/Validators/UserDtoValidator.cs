using CShop.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Validators
{
    public class UserDtoValidator: AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(u => u.PlainPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).When(u => !string.IsNullOrEmpty(u.PlainPassword))
                .WithMessage("Password must be at least 6 characters long.");

            RuleFor(u => u.Profile).SetValidator(new UserProfileDtoValidator());
        }
    }

    public class UserProfileDtoValidator : AbstractValidator<UserProfileDto>
    {
        public UserProfileDtoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(p => p.Phone).NotEmpty().WithMessage("Phone is required.");
            RuleFor(p => p.City).NotEmpty().WithMessage("City is required.");
            RuleFor(p => p.Country).NotEmpty().WithMessage("Country is required.");

            RuleFor(p => p.MiddleName)
                .MaximumLength(50).WithMessage("Middle name must not exceed 50 characters.");
        }
    }

}
