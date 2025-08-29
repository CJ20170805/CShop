using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;

        //Relationships
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

    }
}
