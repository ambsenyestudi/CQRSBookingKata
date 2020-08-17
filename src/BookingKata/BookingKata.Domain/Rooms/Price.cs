namespace BookingKata.Domain.Rooms
{
    public class Price
    {
        public string Currency { get; }
        public double Value { get; }
        public string Description { get; }
        public Price(double value, string currency, string description)
        {
            Value = value;
            Currency = currency;
            Description = description;
        }

    }
}
