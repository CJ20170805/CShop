using AutoMapper;
using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Domain.Entities;
using CShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infrastructure.Services
{
    public class OrderService: IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAppLogger<OrderService> _logger;

        public OrderService(AppDbContext context, IMapper mapper, IAppLogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);

            order.Id = Guid.NewGuid();
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order created with ID: {order.Id}");
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> UpdateAsync(OrderDto dto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == dto.Id);

            if (existingOrder == null)
            {
                _logger.LogWarning($"Order with ID: {dto.Id} not found for update.");
                return null;
            }
            _mapper.Map(dto, existingOrder);
            existingOrder.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {existingOrder.Id} updated.");
            return _mapper.Map<OrderDto>(existingOrder);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                _logger.LogWarning($"Order with ID: {id} not found for deletion.");
                return false;
            }
            _context.Orders.Remove(existingOrder);

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {id} deleted.");
            return true;
        }
    }
}
