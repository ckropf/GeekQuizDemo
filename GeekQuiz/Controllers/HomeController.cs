using GeekQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekQuiz.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TriviaContext _db = new TriviaContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _db.Dispose(); }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult AllQuestions()
        {
            return View(_db.TriviaQuestions.ToList());
        }
    }
}