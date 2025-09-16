using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.ValueObjects
{
    public sealed class Stock: IEquatable<Stock>
    {
        public int Quantity { get; private set; }
        private Stock() { }
        public Stock(int quantity)
        {
            if (quantity < 0) throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));
            Quantity = quantity;
        }

        public static Stock operator +(Stock a, Stock b) => a.Add(b);
        public static Stock operator -(Stock a, Stock b) => a.Subtract(b);
        public Stock Add(Stock other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return new Stock(Quantity + other.Quantity);
        }
        public Stock Subtract(Stock other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (Quantity < other.Quantity) throw new InvalidOperationException("Resulting stock cannot be negative");
            return new Stock(Quantity - other.Quantity);
        }


        public override bool Equals(object? obj) => Equals(obj as Stock);
        public bool Equals(Stock? other)
        {
            if (other is null) return false;
            return Quantity == other.Quantity;
        }
        public override int GetHashCode() => Quantity.GetHashCode();
        public override string ToString() => Quantity.ToString();
    }
}
