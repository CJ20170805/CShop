﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.Entities
{
    public class ProductTag
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public Guid TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
