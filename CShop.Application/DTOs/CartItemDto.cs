using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public string Currency { get; set; } = "USD";

        public decimal SubTotal { get; set; }

        public string? ImageUrl { get; set; }
    }
}
