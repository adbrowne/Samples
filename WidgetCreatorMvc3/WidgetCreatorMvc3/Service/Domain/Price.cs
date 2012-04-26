namespace WidgetCreatorMvc3.Service.Domain
{
    using WidgetCreatorMvc3.Service.Core;

    public class Price
    {
        public decimal Amount { get; private set; }

        public Currency Currency { get; private set; }

        public Price(decimal amount, Currency currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }
    }
}