using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CShop.Domain.Entities;
using CShop.Domain.ValueObjects;
using AutoMapper;

namespace CShop.Infrastructure.Services
{
    public class ProductService: IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        
        public ProductService(AppDbContext context, IMapper mapper)
        {            
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ProductDto?>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            dto.Id = product.Id;
            return dto;
        }

        public async Task<ProductDto?> UpdateAsync(ProductDto dto)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (product == null) return null;

            _mapper.Map(dto, product);

            product.UpdatedAt = DateTime.UtcNow;

            var dtoImageIds = dto.ProductImages.Select(p => p.Id).ToList();

            // Remove images that were deleted
            foreach (var img in product.ProductImages.ToList())
            {
                if (!dtoImageIds.Contains(img.Id))
                {
                    _context.ProductImages.Remove(img);
                }
            }

            // Add or Update images
            foreach (var imgDto in dto.ProductImages)
            {
                var existingImg = product.ProductImages
                    .FirstOrDefault(pi => pi.Id == imgDto.Id);

                if (existingImg != null)
                {
                    // Update existing image
                    existingImg.ImageUrl = imgDto.ImageUrl;
                    existingImg.IsPrimary = imgDto.IsPrimary;
                }
                else
                {
                    // Add new image
                    var newImg = _mapper.Map<ProductImage>(imgDto);
                    product.ProductImages.Add(newImg);
                }
            }

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
