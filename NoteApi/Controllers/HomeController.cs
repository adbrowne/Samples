using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NoteApi.Domain;

namespace NoteApi.Controllers
{
    public class HomeController : Controller
    {
        private ISession session;
        public HomeController(UnitOfWork uow, ISession session)
        {
            this.session = session;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            session.Save(new Image { Name = Guid.NewGuid().ToString() });

            var images = session.CreateQuery("From Image").Enumerable<Image>().ToList();
           
            return View(images);
        }

    }
}
