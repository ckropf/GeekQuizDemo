using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
    [Authorize]
    public class TriviaQuestionsController : Controller
    {
        private readonly TriviaContext _db = new TriviaContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _db.Dispose(); }
            base.Dispose(disposing);
        }

        // GET: TriviaQuestions
        public ActionResult Index()
        {
            return View(_db.TriviaQuestions.ToList());
        }

        // GET: TriviaQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaQuestion triviaQuestion = _db.TriviaQuestions.Find(id);
            if (triviaQuestion == null)
            {
                return HttpNotFound();
            }
            return View(triviaQuestion);
        }

        // GET: TriviaQuestions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TriviaQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] TriviaQuestion triviaQuestion)
        {
            if (ModelState.IsValid)
            {
                _db.TriviaQuestions.Add(triviaQuestion);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(triviaQuestion);
        }

        // GET: TriviaQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaQuestion triviaQuestion = _db.TriviaQuestions.Find(id);
            if (triviaQuestion == null)
            {
                return HttpNotFound();
            }
            return View(triviaQuestion);
        }

        // POST: TriviaQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] TriviaQuestion triviaQuestion)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(triviaQuestion).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(triviaQuestion);
        }

        // GET: TriviaQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaQuestion triviaQuestion = _db.TriviaQuestions.Find(id);
            if (triviaQuestion == null)
            {
                return HttpNotFound();
            }
            return View(triviaQuestion);
        }

        // POST: TriviaQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TriviaQuestion triviaQuestion = _db.TriviaQuestions.Find(id);
            _db.TriviaQuestions.Remove(triviaQuestion);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
