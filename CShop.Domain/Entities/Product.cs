using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        // Optional SKU for real-world uniqueness
        public string? SKU { get; set; }
        public string? Barcode { get; set; }

        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}
