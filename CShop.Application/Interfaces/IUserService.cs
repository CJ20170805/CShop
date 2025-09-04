using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.Application.DTOs;

namespace CShop.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto> CreateAsync(UserDto userDto);
        Task<UserDto?> UpdateAsync(UserDto userDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
