using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

        public decimal Total => Items.Sum(i => i.SubTotal);
    }
}
