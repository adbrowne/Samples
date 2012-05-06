namespace WidgetServices.Mvc.Widget
{
    using System;
    using System.Web.Mvc;

    using WidgetServices.Services.Approval;

    public class WidgetController : Controller
    {
        private readonly IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            this._widgetService = widgetService;
        }

        public ActionResult Index(Guid id)
        {
            var widgetDetails = this._widgetService.GetWidgetDetails(id);
            return this.View(new ViewWidgetViewModel
                {
                    Title = widgetDetails.Title
                });
        }

        public ActionResult List()
        {
            return this.View(new WidgetListViewModel
            {
                Widgets = this._widgetService.GetWidgets()
            });
        }

        [HttpPost]
        public ActionResult Index(Guid id, WidgetDetails widgetDetails)
        {
            widgetDetails.ApprovalId = id;
            this._widgetService.SetWidgetDetails(widgetDetails);
            return this.RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return this.View(new CreateWidgetViewModel
                {
                    ApprovalId = Guid.NewGuid()
                });
        }

        [HttpPost]
        public ActionResult Create(WidgetDetails widgetDetails)
        {
            this._widgetService.SetWidgetDetails(widgetDetails);
            return this.RedirectToAction("Index", new { id = widgetDetails.ApprovalId });
        }
    }
}
