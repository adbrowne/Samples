namespace WidgetCreatorMvc.Models.Widget
{
    using WidgetCreatorMvc3.Service.DTO;

    public class WidgetViewViewModel
    {
        private readonly WidgetDetail widgetDetail;

        public WidgetViewViewModel(WidgetDetail widgetDetail)
        {
            this.widgetDetail = widgetDetail;
        }

        public string Id
        {
            get
            {
                return this.widgetDetail.Id.ToString();
            }
        }

        public string Title
        {
            get
            {
                return this.widgetDetail.Title;
            }
        }

        public string Price
        {
            get
            {
                return this.widgetDetail.Price.Amount + " " + this.widgetDetail.Price.Currency;
            }
        }
    }
}