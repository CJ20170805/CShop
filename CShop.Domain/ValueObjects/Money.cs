using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Domain.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "USD";

        private Money() { }
        public Money(decimal amount, string currency = "USD")
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty", nameof(currency));
           
            Amount = decimal.Round(amount, 2);
            Currency = currency.ToUpperInvariant();
        }

        public static Money FromUSD(decimal amount) => new(amount, "USD");
        public static Money FromEUR(decimal amount) => new(amount, "EUR");
        public static Money FromGBP(decimal amount) => new(amount, "GBP");
        public static Money FromCAD(decimal amount) => new(amount, "CAD");

        public static Money operator +(Money a, Money b) => a.Add(b);
        public static Money operator -(Money a, Money b) => a.Subtract(b);

        public Money Add(Money other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (Currency != other.Currency) throw new InvalidOperationException("Cannot add amounts with different currencies");
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (Currency != other.Currency) throw new InvalidOperationException("Cannot subtract amounts with different currencies");
            if (Amount < other.Amount) throw new InvalidOperationException("Resulting amount cannot be negative");
            return new Money(Amount - other.Amount, Currency);
        }

        public override bool Equals(object? obj) => Equals(obj as Money);
        public bool Equals(Money? other)
        {
            if (other is null) return false;
            return Amount == other.Amount && Currency == other.Currency;
        }
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);

        public override string ToString() => $"{Amount} {Currency}";
        public string ToString(IFormatProvider culture) => Amount.ToString("C", culture) + $" {Currency}";

    }
}
