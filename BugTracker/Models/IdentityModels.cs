using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Доп.поля
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }

        //Db
        public ICollection<Bug> Bugs { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Bug
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Headline { get; set; }
        public string Severity { get; set; }
        public string ErrorType { get; set; }
        public string Description { get; set; }
        public string ActualResult { get; set; }
        public string ExpectedResult { get; set; }
        //Attachment

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Bug> Bugs { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {  
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}