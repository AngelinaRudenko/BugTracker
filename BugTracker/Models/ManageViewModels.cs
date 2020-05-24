using System.ComponentModel.DataAnnotations;
using BugTracker.Resources.Entities.ApplicationUser;
using BugTracker.Resources.Validation;

namespace BugTracker.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("Password", NameResourceType = typeof(Fields))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Errors), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("NewPassword", NameResourceType = typeof(Fields))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayNameAttribute("ConfirmPassword", NameResourceType = typeof(Fields))]
        [Compare("NewPassword", ErrorMessageResourceName = "MustMatch", ErrorMessageResourceType = typeof(Errors))]
        public string ConfirmPassword { get; set; }
    }
}