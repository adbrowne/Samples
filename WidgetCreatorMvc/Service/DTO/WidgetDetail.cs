namespace WidgetCreatorMvc.Service.DTO
{
    using System;

    public class WidgetDetail
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Price Price { get; set; }
    }
}