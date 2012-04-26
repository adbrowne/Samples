namespace WidgetCreatorMvc3.Service.DTO
{
    using WidgetCreatorMvc3.Service.Core;

    public class WidgetTitleUpdate
    {
        public WidgetId Id { get; set; }

        public string NewTitle { get; set; }
    }
}