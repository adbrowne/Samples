namespace WidgetCreatorMvc.Service.Domain
{
    using WidgetCreatorMvc.Controllers;

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