namespace WidgetCreatorMvc.Service.DTO
{
    using WidgetCreatorMvc.Service.Domain;

    public class Price
    {
        public decimal Amount { get; set; }

        public Currency Currency { get; set; }
    }
}