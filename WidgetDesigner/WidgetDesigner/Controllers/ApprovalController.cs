using System.Web.Mvc;

namespace WidgetDesigner.Controllers
{
    using System;

    using WidgetDesigner.Contract;
    using WidgetDesigner.Models;
    using WidgetDesigner.Services.Approval;

    public class ApprovalController : Controller
    {
        private readonly IBus _bus;

        public ApprovalController(IBus bus)
        {
            this._bus = bus;
        }

        //
        // GET: /Approval/
        public ActionResult Index(Guid id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new CreateApprovalViewModel
                {
                    ApprovalId = Guid.NewGuid()
                });
        }

        [HttpPost]
        public ActionResult Create(CreateApproval createApproval)
        {
            var callback = _bus.Send(createApproval);
            callback.Register(_ => { }, null).AsyncWaitHandle.WaitOne(5000);
            return RedirectToAction("Index", new { id = createApproval.ApprovalId });
        }
    }
}
