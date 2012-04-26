namespace WidgetCreatorMvc.Service.Domain
{
    using System;

    public class Widget
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; }

        public Price Price { get; private set; }

        public Widget(Guid id, string title, Price cost)
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