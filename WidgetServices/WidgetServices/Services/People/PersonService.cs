namespace WidgetServices.Services.People
{
    using System;
    using System.Collections.Generic;

    using WidgetServices.Mvc.Widget;

    class PersonService : IPersonService
    {
        public IEnumerable<Person> GetPeople()
        {
            yield return new Person { Id = new Guid("E7B4E67D-A2A5-4E33-BCBC-FD02B70EAF13"), Name = "John" };
            yield return new Person { Id = new Guid("2EAA9E5A-1EEC-45B0-88DB-37143DF5FD43"), Name = "Paul" };
            yield return new Person { Id = new Guid("232B32C7-57D8-4B91-A238-E19A6E19E7FE"), Name = "Ringo" };
            yield return new Person { Id = new Guid("D37B17B8-094B-4A5B-9922-0C88633D81EF"), Name = "George" };
        }
    }
}