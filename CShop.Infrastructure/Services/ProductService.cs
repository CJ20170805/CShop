using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CShop.Infrastructure.Services
{
    public class ProductService: IProductService
    {
        private readonly AppDbContext _context;
        
        public ProductService(AppDbContext context) => _context = context;

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }


    }
}
