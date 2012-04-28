namespace WidgetCreatorMvc3.Service.DTO
{
    using System.ComponentModel.DataAnnotations;

    using WidgetCreatorMvc3.Service.Core;

    public class WidgetTitleUpdate
    {
        public WidgetId Id { get; set; }

        [Required]
        public string NewTitle { get; set; }
    }
}