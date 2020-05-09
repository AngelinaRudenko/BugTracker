using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TeamController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        List<ApplicationUser> members = new List<ApplicationUser>();

        public ActionResult Index()
        {
            string myId = User.Identity.GetUserId();
            var teams = db.Teams.Where(t => t.Users.FirstOrDefault(u => u.Id == myId) != null);
            return View(teams);
        }

        [HttpGet]
        public ActionResult CreateTeam()
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
        public ActionResult CreateTeam(Team team)
        {
            team.Id = db.Teams.Count() + 1;
            string myId = User.Identity.GetUserId();
            ApplicationUser me = db.Users.FirstOrDefault(u => u.Id == myId);
            team.Users.Add(me);
            db.Teams.Add(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateTeam(Team team, string id)
        {
           
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Label = "Edit team";
            Team team =db.Teams.FirstOrDefault(x => x.Id == id);
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
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Team team, string addMember, string removeMember)
        {
            if (!String.IsNullOrEmpty(addMember))
            {
                Team oldTeam = db.Teams.FirstOrDefault(x => x.Id == team.Id);
                if (oldTeam != null)
                {
                    oldTeam.Name = team.Name;
                    oldTeam.Projects = team.Projects;
                    oldTeam.Users.Add(db.Users.FirstOrDefault(x => x.Id == addMember)); ;
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Edit");
            }
            else if (!String.IsNullOrEmpty(removeMember))
            {
                Team oldTeam = db.Teams.FirstOrDefault(x => x.Id == team.Id);
                if (oldTeam != null)
                {
                    oldTeam.Name = team.Name;
                    oldTeam.Projects = team.Projects;
                    oldTeam.Users.Remove(db.Users.FirstOrDefault(x => x.Id == removeMember));
                    db.SaveChanges();

                }
                return RedirectToAction("Edit");
            }
            else
            {
                Team oldTeam = db.Teams.FirstOrDefault(x => x.Id == team.Id);
                if (oldTeam != null)
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
    }
}