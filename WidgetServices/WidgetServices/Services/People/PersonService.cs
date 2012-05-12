namespace WidgetServices.Services.People
{
    using System.Collections.Generic;

    public class PersonService : IPersonService
    {
        public IEnumerable<Person> GetPeople()
        {
            yield return new Person { Id = Constants.JohnPersonId, Name = "John" };
            yield return new Person { Id = Constants.PaulPersonId, Name = "Paul" };
            yield return new Person { Id = Constants.RingoPersonId, Name = "Ringo" };
            yield return new Person { Id = Constants.GeorgePersonId, Name = "George" };
        }
    }
}