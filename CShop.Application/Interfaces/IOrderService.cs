using CShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<OrderDto> CreateAsync(OrderDto dto);
        Task<OrderDto?> UpdateAsync(OrderDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
