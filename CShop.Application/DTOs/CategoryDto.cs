using CShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? ParentCategoryId { get; set; }

        public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();
    }
}
