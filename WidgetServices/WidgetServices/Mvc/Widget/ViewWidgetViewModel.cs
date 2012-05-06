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

        public IEnumerable<SelectListItem> ApproverSelectList { get; set; }

        public string VersionId { get; set; }

        public IEnumerable<Guid> Viewers { get; set; }
        
        public IEnumerable<SelectListItem> ViewersSelectList { get; set; }
    }
}