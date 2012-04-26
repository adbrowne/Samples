namespace WidgetCreatorMvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using WidgetCreatorMvc.Models.Widget;
    using WidgetCreatorMvc.Service;
    using WidgetCreatorMvc.Service.DTO;

    public class WidgetController : Controller
    {
        private readonly WidgetService service;

        public WidgetController(WidgetService service)
        {
            this.service = service;
        }

        public ViewResult Index()
        {
            var viewModel = new WidgetListViewModel(service.AllWidgets());
            return this.View(viewModel);
        }

        public ActionResult View(Guid id)
        {
            var viewModel = new WidgetViewViewModel(service.GetWidget(id));
            return View(viewModel);
        }

        public ActionResult UpdateTitle(Guid id)
        {
            var viewModel = new WidgetUpdateTitleViewModel(service.GetWidget(id));
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateTitle(WidgetTitleUpdate update)
        {
            service.UpdateWidgetTitle(update);
            return RedirectToAction("Index");
        }
    }
}