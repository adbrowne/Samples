using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;

namespace ConstructorInjectionTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession session;

        public class ViewModel
        {
            public string Message { get; set; }
        }

        public HomeController(ISession session)
        {
            this.session = session;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            var foo = session.CreateCriteria<Foo>().List<Foo>().First();
            return View(new ViewModel{ Message = foo.GetFormattedName() });
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
