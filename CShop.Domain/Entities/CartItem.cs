using CShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        // Pirce snapshot at the time of adding to cart
        public Money UnitPrice { get; set; } = null!;

        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;

        private CartItem() { }
        public CartItem(Guid cartId, Guid productId, int quantity, Money unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            if (unitPrice == null) throw new ArgumentNullException(nameof(unitPrice));

            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
            Quantity = newQuantity;
        }

        public Money SubTotal => new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);
    }
}
