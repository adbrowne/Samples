namespace WidgetCreatorMvc3.Service.Domain
{
    using WidgetCreatorMvc3.Service.Core;

    public class Widget
    {
        public WidgetId Id { get; private set; }

        public string Title { get; private set; }

        public Price Price { get; private set; }

        public Widget(WidgetId id, string title, Price cost)
        {
            this.Id = id;
            this.Title = title;
            this.Price = cost;
        }

        public void UpdateTitle(string newTitle)
        {
            this.Title = newTitle;
        }
    }
}