using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugTracker.Resources.Entities.ApplicationUser;
using BugTracker.Resources.Validation;

namespace BugTracker.Models
{
     public class LoginViewModel
    {
        [Required]
        [LocalizedDisplayNameAttribute("Email", NameResourceType =typeof(Fields))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("Password", NameResourceType = typeof(Fields))]
        public string Password { get; set; }

        [LocalizedDisplayNameAttribute("RememberMe", NameResourceType = typeof(Fields))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [LocalizedDisplayNameAttribute("Email", NameResourceType = typeof(Fields))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Errors), MinimumLength = 3)]    
        [LocalizedDisplayNameAttribute("Name", NameResourceType = typeof(Fields))]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Errors), MinimumLength = 3)]
        [LocalizedDisplayNameAttribute("Surname", NameResourceType = typeof(Fields))]
        public string Surname { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Errors), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("Password", NameResourceType = typeof(Fields))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("ConfirmPassword", NameResourceType = typeof(Fields))]
        [Compare("Password", ErrorMessageResourceName = "MustMatch", ErrorMessageResourceType = typeof(Errors))]
        public string ConfirmPassword { get; set; }
    }
}
