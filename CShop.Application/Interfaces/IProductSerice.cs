using CShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        //Task<ProductDto> GetByIdAsync(Guid id);
        //Task<Guid> CreateAsync(ProductDto product);
        //Task UpdateAsync(ProductDto product);
        //Task DeleteAsync(Guid id);
    }
}
