using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.ValueObjects
{
    public class ShippingAddress
    {
        public string RecipientName { get; private set; } = null!;
        public string AddressLine1 { get; private set; } = null!;
        public string? AddressLine2 { get; private set; } = null;
        public string City { get; private set; } = null!;
        public string? State { get; private set; } = null;
        public string PostalCode { get; private set; } = null!;
        public string Country { get; private set; } = null!;

        private ShippingAddress() { }
        public ShippingAddress(
            string recipientName,
            string addressLine1, 
            string? addressLine2, 
            string city,
            string? state, 
            string postalCode, 
            string country)
        {
            RecipientName = recipientName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }
    }
}
