using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class BugController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bug
        public ActionResult Index()
        {
            //Ленивая подгрузка - загрузка по требованию
            //var id = User.Identity.GetUserId();
            ////var bugs = db.Bugs.Where(x => x.ApplicationUser.Id == id).ToList();
            //var bugs = db.Bugs.Where(x => x.ApplicationUserId == id).ToList();
            //return View(bugs);
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Label = "Add new bug";
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Add(Bug bug)
        {
            //bug.ApplicationUserId = User.Identity.GetUserId();
            bug.Id = db.Bugs.Count() + 1;
            bug.Date = DateTime.Now;
            db.Bugs.Add(bug);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Bug bug = db.Bugs.FirstOrDefault(x => x.Id == id);
            if (bug != null)
            {
                ViewBag.Label = "Edit bug";
                return View(bug);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bug bug)
        {
            Bug oldBug = db.Bugs.FirstOrDefault(x => x.Id == bug.Id);
            if (oldBug != null)
            {
                oldBug.Date = DateTime.Now;
                oldBug.Headline = bug.Headline;
                oldBug.Severity = bug.Severity;
                oldBug.ErrorType = bug.ErrorType;
                oldBug.Description = bug.Description;
                oldBug.ActualResult = bug.ActualResult;
                oldBug.ExpectedResult = bug.ExpectedResult;
                //oldBug.ApplicationUserId = bug.ApplicationUserId;
                db.Entry(oldBug).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Bug bug = db.Bugs.FirstOrDefault(x => x.Id == id);
            if (bug != null)
            {
                db.Bugs.Remove(bug);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}