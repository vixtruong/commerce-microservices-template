using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Common
{
    public sealed class Money : IEquatable<Money>
    {
        private Money()
        {
            Currency = string.Empty;
        }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public static Result<Money> Create(decimal amount, string currency)
        {
            if (amount < 0)
            {
                return MoneyErrors.AmountNegative;
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                return MoneyErrors.CurrencyRequired;
            }

            string normalizedCurrency = currency.Trim().ToUpperInvariant();

            if (normalizedCurrency.Length != 3)
            {
                return MoneyErrors.CurrencyInvalid;
            }

            return new Money(decimal.Round(amount, 2), normalizedCurrency);
        }

        public bool Equals(Money? other) =>
            other is not null &&
            Amount == other.Amount &&
            string.Equals(Currency, other.Currency, StringComparison.Ordinal);

        public override bool Equals(object? obj) => Equals(obj as Money);

        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    }

}
