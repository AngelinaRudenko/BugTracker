using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string myId = User.Identity.GetUserId();
            var projects = db.Projects.Where(t => t.Team.Users.FirstOrDefault(x => x.Id == myId) != null).Include(x => x.Team);
            return View(projects);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Label = "Create new project";
            string myId = User.Identity.GetUserId();
            ApplicationUser me = db.Users.FirstOrDefault(u => u.Id == myId);
            List<Team> teams = new List<Team>();
            if (me.Teams.Count != 0)
            {
                foreach (Team team in me.Teams)
                {
                    teams.Add(db.Teams.FirstOrDefault(x => x.Id == team.Id));
                }
            }
            ViewBag.Teams = teams;
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (!String.IsNullOrEmpty(project.TeamId.ToString()))
            {
                project.Team = db.Teams.FirstOrDefault(x => x.Id == project.TeamId);
            }
            db.Projects.Add(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Label = "Edit project";
            Project project = db.Projects.FirstOrDefault(x => x.Id == id);
            string myId = User.Identity.GetUserId();
            ApplicationUser me = db.Users.FirstOrDefault(u => u.Id == myId);
            List<Team> teams = new List<Team>();
            if (me.Teams.Count != 0)
            {
                foreach (Team team in me.Teams)
                {
                    teams.Add(db.Teams.FirstOrDefault(x => x.Id == team.Id));
                }
            }
            ViewBag.Teams = teams;
            return View(project);
        }

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            Project oldProject = db.Projects.FirstOrDefault(x => x.Id == project.Id);
            if (oldProject != null)
            {
                oldProject.Name = project.Name;
                oldProject.TeamId = project.TeamId;
                oldProject.Team = db.Teams.FirstOrDefault(x => x.Id == project.TeamId);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.FirstOrDefault(x => x.Id == id);
            if (project != null)
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}