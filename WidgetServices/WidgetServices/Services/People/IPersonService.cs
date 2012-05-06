namespace WidgetServices.Services.People
{
    using System.Collections.Generic;

    using WidgetServices.Mvc.Widget;

    public interface IPersonService
    {
        IEnumerable<Person> GetPeople();
    }
}