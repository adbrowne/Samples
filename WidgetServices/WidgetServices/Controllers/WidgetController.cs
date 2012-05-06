using System;
using System.Web.Mvc;

namespace WidgetServices.Controllers
{
    using System.Collections.Generic;

    using WidgetServices.Services.Approval;

    public class WidgetController : Controller
    {
        private readonly IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        //
        // GET: /Approval/

        public ActionResult Index(Guid id)
        {
            var widgetDetails = _widgetService.GetWidgetDetails(id);
            return View(new ViewWidgetViewModel
                {
                    Title = widgetDetails.Title
                });
        }

        public ActionResult List()
        {
            return View(new WidgetListViewModel
            {
                Widgets = _widgetService.GetWidgets()
            });
        }

        [HttpPost]
        public ActionResult Index(Guid id, WidgetDetails widgetDetails)
        {
            widgetDetails.ApprovalId = id;
            _widgetService.SetWidgetDetails(widgetDetails);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new CreateWidgetViewModel
                {
                    ApprovalId = Guid.NewGuid()
                });
        }

        [HttpPost]
        public ActionResult Create(WidgetDetails widgetDetails)
        {
            _widgetService.SetWidgetDetails(widgetDetails);
            return RedirectToAction("Index", new { id = widgetDetails.ApprovalId });
        }
    }

    public class WidgetListViewModel
    {
        public IEnumerable<WidgetDetails> Widgets { get; set; }
    }

    public class ViewWidgetViewModel
    {
        public string Title { get; set; }
    }

    public class CreateWidgetViewModel
    {
        public Guid ApprovalId { get; set; }
        public string Title { get; set; }
    }
}
