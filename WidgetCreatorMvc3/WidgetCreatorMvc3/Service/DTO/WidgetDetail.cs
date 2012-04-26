namespace WidgetCreatorMvc3.Service.DTO
{
    using WidgetCreatorMvc3.Service.Core;

    using Price = WidgetCreatorMvc3.Service.DTO.Price;

    public class WidgetDetail
    {
        public WidgetId Id { get; set; }

        public string Title { get; set; }

        public DTO.Price Price { get; set; }
    }
}