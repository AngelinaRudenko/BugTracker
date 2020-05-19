using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class BugController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int id)
        {
            List<Bug> bugs = db.Bugs.Where(x => x.ProjectId == id).ToList();
            ViewBag.Project = db.Projects.FirstOrDefault(x => x.Id == id);
            return View(bugs);
        }

        [HttpGet]
        public ActionResult Add(int projectId, string projectName)
        {
            ViewBag.ProjectName = projectName;
            ViewBag.ProjectId = projectId;
            ViewBag.Label = "Add new bug";
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Add(Bug bug, int projectId)
        {
            bug.ProjectId = projectId;
            bug.Date = DateTime.Now;
            db.Bugs.Add(bug);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = projectId });
        }

        [HttpGet]
        public ActionResult Edit(int id, int projectId, string projectName)
        {
            Bug bug = db.Bugs.FirstOrDefault(x => x.Id == id);
            if (bug != null)
            {
                ViewBag.Label = "Edit bug";
                ViewBag.ProjectName = projectName;
                ViewBag.ProjectId = projectId;
                return View(bug);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bug bug, int projectId)
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
                db.Entry(oldBug).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { Id = projectId });
        }

        [HttpGet]
        public ActionResult Delete(int id, int projectId)
        {
            Bug bug = db.Bugs.FirstOrDefault(x => x.Id == id);
            if (bug != null)
            {
                db.Bugs.Remove(bug);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = projectId });
            }
            return HttpNotFound();
        }
    }
}