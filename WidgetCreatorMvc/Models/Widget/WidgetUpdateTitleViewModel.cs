namespace WidgetCreatorMvc.Models.Widget
{
    using WidgetCreatorMvc.Service.DTO;

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
                return widgetDetail.Title;
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
                return widgetDetail.Id.ToString();
            }
        }
    }
}