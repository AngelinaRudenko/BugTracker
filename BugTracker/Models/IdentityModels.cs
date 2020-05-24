using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker;

namespace BugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [LocalizedDisplayNameAttribute("Name", NameResourceType = typeof(Resources.Entities.ApplicationUser.Fields))]
        public string Name { get; set; }
        [Required]
        [LocalizedDisplayNameAttribute("Surname", NameResourceType = typeof(Resources.Entities.ApplicationUser.Fields))]
        public string Surname { get; set; }
        [LocalizedDisplayNameAttribute("Age", NameResourceType = typeof(Resources.Entities.ApplicationUser.Fields))]
        public string Age { get; set; }
        [LocalizedDisplayNameAttribute("Country", NameResourceType = typeof(Resources.Entities.ApplicationUser.Fields))]
        public string Country { get; set; }
        [LocalizedDisplayNameAttribute("Company", NameResourceType = typeof(Resources.Entities.ApplicationUser.Fields))]
        public string Company { get; set; }
        public virtual ICollection<FriendAssociation> FriendAssociations { get; set; }
        public virtual ICollection<Team> Teams { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);           
            return userIdentity;// Add custom user claims here
        }
    }

    public class Bug
    {
        public int Id { get; set; }
        [LocalizedDisplayNameAttribute("Date", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public DateTime Date { get; set; }
        [LocalizedDisplayNameAttribute("Headline", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string Headline { get; set; }
        [LocalizedDisplayNameAttribute("Severity", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string Severity { get; set; }
        [LocalizedDisplayNameAttribute("ErrorType", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string ErrorType { get; set; }
        [LocalizedDisplayNameAttribute("Description", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string Description { get; set; }
        [LocalizedDisplayNameAttribute("ActualResult", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string ActualResult { get; set; }
        [LocalizedDisplayNameAttribute("ExpectedResult", NameResourceType = typeof(Resources.Entities.Bug.Fields))]
        public string ExpectedResult { get; set; }
        //Attachment
        public int ProjectId { get; set; }
        public  Project Project { get; set; }
    }

    public class FriendAssociation
    {
        public int Id { get; set; }
        public bool IsAFriend { get; set; }
        public string ApplicationUserId { get; set; } //To assign User
        public ApplicationUser ApplicationUser { get; set; }
        public string FriendId { get; set; } //To assign friend
    }

    public class Team
    {
        public int Id { get; set; }
        [LocalizedDisplayNameAttribute("Name", NameResourceType = typeof(Resources.Entities.Team.Fields))]
        public string Name { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Team()
        {
            Projects = new List<Project>();
            Users = new List<ApplicationUser>();
        }
    }

    public class Project
    {
        public int Id { get; set; }
        [LocalizedDisplayNameAttribute("Name", NameResourceType = typeof(Resources.Entities.Project.Fields))]
        public string Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public virtual ICollection<Bug> Bugs { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<FriendAssociation> FriendAssociations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}