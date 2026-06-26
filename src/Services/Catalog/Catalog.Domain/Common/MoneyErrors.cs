using Commerce.BuildingBlocks.Domain.Results;

namespace Catalog.Domain.Common
{
    /// <summary>
    /// Provides centralized money value object error definitions for the Catalog domain.
    /// </summary>
    public static class MoneyErrors
    {
        /// <summary>
        /// Error returned when a money amount is negative.
        /// </summary>
        public static readonly Error AmountNegative = Error.Validation(
            "Catalog.Money.AmountNegative",
            "Money amount must not be negative.");

        /// <summary>
        /// Error returned when a currency code is required but was not supplied.
        /// </summary>
        public static readonly Error CurrencyRequired = Error.Validation(
            "Catalog.Money.CurrencyRequired",
            "Currency is required.");

        /// <summary>
        /// Error returned when a currency code does not match the expected ISO-style length.
        /// </summary>
        public static readonly Error CurrencyInvalid = Error.Validation(
            "Catalog.Money.CurrencyInvalid",
            "Currency must contain exactly three characters.");
    }
}
