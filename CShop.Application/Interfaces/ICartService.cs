using CShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetByUserIdAsync(Guid userId);
        Task<CartDto> AddItemAsync(Guid userId, Guid productId, int quantity);
        Task<CartDto> UpdateItemAsync(Guid userId, Guid productId, int quantity);
        Task<CartDto> RemoveItemAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
    }
}
