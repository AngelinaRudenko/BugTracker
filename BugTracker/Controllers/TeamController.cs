using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string myId = User.Identity.GetUserId();
            var teams = db.Teams.Where(t => t.Users.FirstOrDefault(u => u.Id == myId) != null);
            return View(teams);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Label = "Create new team";
            string myId = User.Identity.GetUserId();
            ApplicationUser me = db.Users.FirstOrDefault(u => u.Id == myId);
            List<ApplicationUser> friends = new List<ApplicationUser>();
            if (me.FriendAssociations.Count != 0)
            {
                foreach (FriendAssociation fa in me.FriendAssociations)
                {
                    if (fa.IsAFriend)
                        friends.Add(db.Users.FirstOrDefault(x => x.Id == fa.FriendId));
                }
            }
            ViewBag.Friends = friends;
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(Team team, string addMember)
        {
            string myId = User.Identity.GetUserId();
            team.Users.Add(db.Users.FirstOrDefault(u => u.Id == myId));
            if (!String.IsNullOrEmpty(addMember))
            {
                team.Users.Add(db.Users.FirstOrDefault(x => x.Id == addMember));
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = team.Id });
            }
            db.Teams.Add(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Label = "Edit team";
            Team team = db.Teams.FirstOrDefault(x => x.Id == id);
            string myId = User.Identity.GetUserId();
            ApplicationUser me = db.Users.FirstOrDefault(u => u.Id == myId);
            List<ApplicationUser> friends = new List<ApplicationUser>();
            List<ApplicationUser> members = new List<ApplicationUser>();
            if (me.FriendAssociations.Count != 0)
            {
                foreach (FriendAssociation fa in me.FriendAssociations)
                {
                    if (fa.IsAFriend)
                    {
                        if (team.Users.FirstOrDefault(x => x.Id == fa.FriendId) == null)
                        {
                            friends.Add(db.Users.FirstOrDefault(x => x.Id == fa.FriendId));
                        }
                        else
                        {
                            members.Add(db.Users.FirstOrDefault(x => x.Id == fa.FriendId));
                        }
                    }
                }
            }
            ViewBag.Friends = friends;
            ViewBag.Members = members;
            return View(team);
        }

        [HttpPost]
        public ActionResult Edit(Team team, string addMember, string removeMember)
        {
            Team oldTeam = db.Teams.FirstOrDefault(x => x.Id == team.Id);
            if (oldTeam != null)
            {
                if (!String.IsNullOrEmpty(addMember))
                {
                    oldTeam.Name = team.Name;
                    oldTeam.Projects = team.Projects;
                    oldTeam.Users.Add(db.Users.FirstOrDefault(x => x.Id == addMember));
                    db.SaveChanges();
                    return RedirectToAction("Edit");
                }
                else if (!String.IsNullOrEmpty(removeMember))
                {
                    oldTeam.Name = team.Name;
                    oldTeam.Projects = team.Projects;
                    oldTeam.Users.Remove(db.Users.FirstOrDefault(x => x.Id == removeMember));
                    db.SaveChanges();
                    return RedirectToAction("Edit");
                }
                else
                {
                    oldTeam.Name = team.Name;
                    oldTeam.Projects = team.Projects;
                    oldTeam.Users = team.Users;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Team team = db.Teams.FirstOrDefault(x => x.Id == id);
            if (team != null)
            {
                db.Teams.Remove(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}