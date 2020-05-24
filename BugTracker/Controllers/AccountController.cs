using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ActionResult Index(string id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            List<ApplicationUser> friends = new List<ApplicationUser>();
            List<ApplicationUser> requests = new List<ApplicationUser>();
            if (user.FriendAssociations.Count != 0)
            {
                foreach (FriendAssociation fa in user.FriendAssociations)
                {
                    if (fa.IsAFriend)
                        friends.Add(db.Users.FirstOrDefault(x => x.Id == fa.FriendId));
                    else
                        requests.Add(db.Users.FirstOrDefault(x => x.Id == fa.FriendId));
                }
            }
            ViewBag.Friends = friends;
            ViewBag.Requests = requests;
            return View(user);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string myId = User.Identity.GetUserId();
                ApplicationUser me = db.Users.FirstOrDefault(x => x.Id == myId);
                List<ApplicationUser> users = new List<ApplicationUser>();
                List<ApplicationUser> friends = new List<ApplicationUser>();
                List<ApplicationUser> requests = new List<ApplicationUser>();
                foreach (ApplicationUser user in db.Users)
                {
                    if ($"{user.Name} {user.Surname}".ToLower().Contains(text.ToLower()) && user.Id != User.Identity.GetUserId())
                    {
                        FriendAssociation fa = me.FriendAssociations.FirstOrDefault(x => x.FriendId == user.Id);
                        if (fa != null)
                        {
                            if (fa.IsAFriend == true)
                                friends.Add(user);
                            else if (fa.IsAFriend == false)
                                requests.Add(user);
                        }
                        else
                            users.Add(user);
                    }
                }
                ViewBag.Friends = friends;
                ViewBag.Requests = requests;
                return View(users);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult AddFriend(string id)
        {
            ApplicationUser personExist = db.Users.FirstOrDefault(x => x.Id == id);
            if (personExist != null)
            {
                string myId = User.Identity.GetUserId();
                ApplicationUser me = db.Users.FirstOrDefault(x => x.Id == myId);
                if (me.FriendAssociations.FirstOrDefault(x => x.FriendId == id) == null)
                {
                    FriendAssociation fa = personExist.FriendAssociations.FirstOrDefault(x => x.FriendId == myId);
                    if (fa == null)
                    {
                        me.FriendAssociations.Add(new FriendAssociation { IsAFriend = false, FriendId = id });
                    }
                    else
                    {
                        fa.IsAFriend = true;
                        me.FriendAssociations.Add(new FriendAssociation { IsAFriend = true, FriendId = id });
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Search");
        }

        [HttpGet]
        public ActionResult Unfriend(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                string myId = User.Identity.GetUserId();
                FriendAssociation fa = db.FriendAssociations.FirstOrDefault(x => x.FriendId == id);
                db.FriendAssociations.Remove(fa);
                ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == id);
                if (user.FriendAssociations != null)
                {
                    FriendAssociation fa2 = user.FriendAssociations.FirstOrDefault(x => x.FriendId == myId);
                    if (fa2 != null)
                        fa2.IsAFriend = false;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = User.Identity.GetUserId()});
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");         
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", BugTracker.Resources.Validation.Errors.LoginFail);
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, Surname = model.Surname };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        Response.Write("Object: " + validationError.Entry.Entity.ToString());
                        Response.Write(" ");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                            Response.Write(err.ErrorMessage + " ");
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}