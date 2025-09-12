using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Domain.Entities;
using CShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CShop.Infrastructure.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            //return await _context.Categories
            //    .Select(c => new CategoryDto
            //    {
            //        Id = c.Id,
            //        Name = c.Name,
            //        ParentCategoryId = c.ParentCategoryId,
            //    })
            //    .ToListAsync();

            var categories = await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.SubCategories)
                .ToListAsync();

            return categories.Select(c => MapCategory(c)).ToList();
        }

        // Recursive mapper
        public CategoryDto MapCategory(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories?
                          .Select(MapCategory)
                          .ToList() ?? new List<CategoryDto>()
            };
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        { 
            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ParentCategoryId = dto.ParentCategoryId,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            dto.Id = category.Id;
            return dto;
        }

        public async Task<CategoryDto?> UpdateAsync(CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.Id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.ParentCategoryId = dto.ParentCategoryId;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public async Task<bool> DeleteAsync (Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
