namespace WidgetServices.Mvc.Widget
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using WidgetServices.Services.People;
    using WidgetServices.Services.Version;
    using WidgetServices.Services.VersionRoles;
    using WidgetServices.Services.WidgetDetails;

    public class WidgetController : Controller
    {
        private readonly IWidgetDetailsService _widgetDetailsService;

        private readonly IPersonService _personService;

        private readonly IVersionService _versionService;

        private readonly IVersionRolesService _versionRolesService;

        public WidgetController(IWidgetDetailsService widgetDetailsService, IPersonService personService, IVersionService versionService, IVersionRolesService versionRolesService)
        {
            this._widgetDetailsService = widgetDetailsService;
            _personService = personService;
            _versionService = versionService;
            _versionRolesService = versionRolesService;
        }

        public ActionResult Index(Guid id)
        {
            var widgetDetails = this._widgetDetailsService.GetWidgetDetails(id);
            var people = _personService.GetPeople().ToList();
            var currentVersion = _versionService.GetCurrentVersion(id);
            var approvers = _versionRolesService.GetRoles(currentVersion.VersionId).ToList();
            
            Debug.Assert(people != null, "people != null");

            return this.View(new ViewWidgetViewModel
                {
                    Title = widgetDetails.Title,
                    People = people,
                    VersionId = currentVersion.VersionId.ToString(),
                    ApproverSelectList = GetPeopleSelectList(approvers, people, VersionRoleType.Approver),
                    ViewersSelectList = GetPeopleSelectList(approvers, people, VersionRoleType.Viewer)
                });
        }

        private static IEnumerable<SelectListItem> GetPeopleSelectList(IEnumerable<VersionRole> currentlySelected, IEnumerable<Person> people, VersionRoleType roleType)
        {
            return people.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                    Selected = currentlySelected.Any(x => x.PersonId == p.Id && x.Role == roleType)
                });
        }

        [HttpPost]
        public ActionResult Index(Guid id, Guid versionId, IEnumerable<Guid> approvers, WidgetDetail widgetDetail, IEnumerable<Guid> viewers)
        {
            widgetDetail.ApprovalId = id;
            this._widgetDetailsService.SetWidgetDetails(widgetDetail);
            _versionRolesService.SetApprovers(versionId, approvers);
            _versionRolesService.SetViewers(versionId, viewers);
            return this.RedirectToAction("Index");
        }

        public ActionResult List()
        {
            return this.View(new WidgetListViewModel
            {
                Widgets = this._widgetDetailsService.GetWidgets()
            });
        }

        public ActionResult Create()
        {
            return this.View(new CreateWidgetViewModel
                {
                    ApprovalId = Guid.NewGuid()
                });
        }

        [HttpPost]
        public ActionResult Create(WidgetDetail widgetDetail)
        {
            this._widgetDetailsService.SetWidgetDetails(widgetDetail);
            return this.RedirectToAction("Index", new { id = widgetDetail.ApprovalId });
        }
    }
}
