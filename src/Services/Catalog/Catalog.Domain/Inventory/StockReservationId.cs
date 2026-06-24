namespace Catalog.Domain.Inventory
{
    public readonly record struct StockReservationId(Guid Value)
    {
        public static StockReservationId New() => new(Guid.NewGuid());

        public static StockReservationId From(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Reservation id must not be empty.", nameof(value));
            }

            return new StockReservationId(value);
        }

        public override string ToString() => Value.ToString();
    }
}
