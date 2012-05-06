namespace WidgetServices.Mvc.Widget
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using WidgetServices.Services.People;

    public class ViewWidgetViewModel
    {
        public string Title { get; set; }

        public IEnumerable<Person> People { get; set; }

        public IEnumerable<Guid> Approvers { get; set; }

        public IEnumerable<SelectListItem> PeopleSelectList
        {
            get
            {
                return People.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            }
        }
    }
}