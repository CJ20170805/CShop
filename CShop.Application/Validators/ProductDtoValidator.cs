using CShop.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Validators
{
    public class ProductDtoValidator: AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Category is required.");

            // Validate ProductImages
            RuleForEach(p => p.ProductImages).ChildRules(images =>
            {
                images.RuleFor(img => img.ImageUrl)
                      .NotEmpty().WithMessage("Image URL is required.");

                images.RuleFor(img => img.IsPrimary)
                      .NotNull().WithMessage("IsPrimary must be specified.");
            });

            // Ensure at least one primary image exists
            RuleFor(p => p.ProductImages)
                .NotEmpty()
                .WithMessage("At least one image is required.")
                .Must(images => images.Any(img => img.IsPrimary))
                //.Must(images => images.Count == 0 || images.Any(img => img.IsPrimary))
                .WithMessage("At least one primary image must be set.");

        }
    }
}
