namespace WidgetCreatorMvc3.Models.Widget
{
    using WidgetCreatorMvc3.Service.DTO;

    public class WidgetUpdateTitleViewModel
    {
        private readonly WidgetDetail widgetDetail;

        public WidgetUpdateTitleViewModel(WidgetDetail widgetDetail)
        {
            this.widgetDetail = widgetDetail;
        }

        public string Title
        {
            get
            {
                return this.widgetDetail.Title;
            }
        }

        public WidgetTitleUpdate Update
        {
            get
            {
                return new WidgetTitleUpdate { NewTitle = this.widgetDetail.Title };
            }
        }

        public string Id
        {
            get
            {
                return this.widgetDetail.Id.ToString();
            }
        }
    }
}