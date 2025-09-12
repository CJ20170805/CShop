using CShop.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Validators
{
    public class CategoryDtoValidator: AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("CategoryName is Required.")
                .MinimumLength(3).WithMessage("CategoryName must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("CategoryName must not exceed 20 characters.");
        }
    }
}
