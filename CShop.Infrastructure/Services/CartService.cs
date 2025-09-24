using AutoMapper;
using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.Domain.ValueObjects;

namespace CShop.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IAppLogger<CartService> _logger;
        private readonly IMapper _mapper;

        public CartService(AppDbContext context, ICacheService cacheService, IAppLogger<CartService> logger, IMapper mapper)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CartDto> GetByUserIdAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}, creating a new one.");
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> AddItemAsync(Guid userId, Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}, creating a new one.");
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                _logger.LogInformation($"Product {productId} already in cart for user {userId}, updating quantity.");
                cartItem.UpdateQuantity(cartItem.Quantity + quantity);
            }
            else
            {
                _logger.LogInformation($"Adding product {productId} to cart for user {userId}.");
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    throw new KeyNotFoundException("Product not found.");

                _logger.LogInformation($"Product {productId} found: {product.Name}, Price: {product.Price.Amount} {product.Price.Currency}");
                var newCartItem = new CartItem(cart.Id, productId, quantity, new Money(product.Price.Amount, product.Price.Currency));
                _context.CartItems.Add(newCartItem);
                //cart.Items.Add(newCartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> UpdateItemAsync(Guid userId, Guid productId, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
                throw new KeyNotFoundException("Cart item not found.");

            cartItem.UpdateQuantity(newQuantity);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
                throw new KeyNotFoundException("Cart item not found.");

            cart.Items.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<CartDto>(cart);
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
