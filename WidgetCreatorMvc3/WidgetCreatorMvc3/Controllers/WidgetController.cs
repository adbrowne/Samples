namespace WidgetCreatorMvc3.Controllers
{
    using System.Web.Mvc;

    using WidgetCreatorMvc.Models.Widget;
    using WidgetCreatorMvc.Service;

    using WidgetCreatorMvc3.Models.Widget;
    using WidgetCreatorMvc3.Service.Core;
    using WidgetCreatorMvc3.Service.DTO;

    public class WidgetController : Controller
    {
        private readonly WidgetService service;

        public WidgetController(WidgetService service)
        {
            this.service = service;
        }

        public ViewResult Index()
        {
            var viewModel = new WidgetListViewModel(this.service.AllWidgets());
            return this.View(viewModel);
        }

        public ActionResult View(WidgetId id)
        {
            var viewModel = new WidgetViewViewModel(this.service.GetWidget(id));
            return this.View(viewModel);
        }

        public ActionResult UpdateTitle(WidgetId id)
        {
            var viewModel = new WidgetUpdateTitleViewModel(this.service.GetWidget(id));
            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateTitle(WidgetTitleUpdate update)
        {
            if (!ModelState.IsValid)
            {
                return UpdateTitle(update.Id);
            }

            this.service.UpdateWidgetTitle(update);
            return this.RedirectToAction("Index");
        }
    }
}