using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Viternus.Data;
using Viternus.Data.Repository;
using System.Configuration.Provider;
using Viternus.Membership;
using Viternus.Web.ViewModels;
using Viternus.Membership.Providers;
using Viternus.Web.Filters;

namespace Viternus.Web.Controllers
{
    [LogError]
    public class AccountController : ApplicationController
    {
        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.
        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl, string facebookId)
        {
            if (!ValidateLogOn(ref userName, password, facebookId))
            {
                ViewData["rememberMe"] = rememberMe;
                return View();
            }

            FormsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //public ActionResult Register()
        //{
        //    return Register(null, null);
        //}

        public ActionResult Register(string email, string userName)
        {
            return View(new AccountRegisterViewModel(MembershipService.MinPasswordLength, MembershipService.MinRequiredNonAlphanumericCharacters, userName, email));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string email, string userName, string newPassword, string confirmPassword, string facebookId, string returnUrl, bool agreedToTOS)
        {
            if (ValidateRegistration(userName, email, facebookId, newPassword, confirmPassword, agreedToTOS))
            {
                MembershipCreateStatus createStatus;
                //TODO: Factor this out... should not be hard-coded
                if ("yourname@example.com" == email.ToLower())
                {
                    createStatus = MembershipCreateStatus.DuplicateEmail;
                }
                else
                {
                    // Attempt to register the user
                    createStatus = MembershipService.CreateUser(userName, facebookId, newPassword, email, agreedToTOS);
                }

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuth.SignIn(userName, false /* createPersistentCookie */);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(new AccountRegisterViewModel(MembershipService.MinPasswordLength, MembershipService.MinRequiredNonAlphanumericCharacters, userName, email));
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ViewData["MinRequiredNonAlphanumericCharacters"] = MembershipService.MinRequiredNonAlphanumericCharacters;

            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ViewData["MinRequiredNonAlphanumericCharacters"] = MembershipService.MinRequiredNonAlphanumericCharacters;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "Your password could not be changed.  Either the current password entered was incorrect or the new password entered was invalid.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit()
        {
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);
            return View(user);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string firstName, string lastName, string nickName, string email, string emailAlternate)
        {
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);

            //TODO: Validate that the user is not changing the address to one that exists on another account

            user.Profile.FirstName = firstName;
            user.Profile.LastName = lastName;
            user.Profile.NickName = nickName;
            if (String.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "Please enter an email address.  It is required.");
            user.Profile.Email = email;
            user.Profile.EmailAlternate = emailAlternate;
            //TODO: If we need SSN, uncomment below
            //ssn = ssn.Replace("-","").Replace(" ","");
            //int tester;
            //if (0 != ssn.Length &&  (9 != ssn.Length || !int.TryParse(ssn, out tester)))
            //    ModelState.AddModelError("ssn", "The Social Security Number you entered was not in the right format.  Please only use numbers with or without dashes (-).");
            //else
            //    user.Profile.SSN = ssn;

            if (!ModelState.IsValid)
                return View(user);

            _userRepository.Save();

            return RedirectToAction("../Home/Index");
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }

            ValidateNewPassword(newPassword, confirmPassword);

            return ModelState.IsValid;
        }

        private void ValidateNewPassword(string newPassword, string confirmPassword)
        {
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a new password of {0} or more characters.",
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("confirmPassword", "The new password and confirmation password do not match.");
            }
        }

        private bool ValidateRegistration(string userName, string email, string facebookId, string password, string confirmPassword, bool agreedToTOS)
        {
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }

            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }

            if (!agreedToTOS)
            {
                ModelState.AddModelError("agreedToTOS", "You must agree to the Privacy Policy and Terms of Service.");
            }

            if (String.IsNullOrEmpty(facebookId))
            {
                ValidateNewPassword(password, confirmPassword);
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(ref string userName, string password, string facebookId)
        {
            if (String.IsNullOrEmpty(facebookId))
            {
                if (String.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("username", "You must specify a username.");
                }
                if (String.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("password", "You must specify a password.");
                }
                try
                {
                    if (ModelState.IsValid && !MembershipService.ValidateUser(userName, password))
                    {
                        ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("_FORM", "Incorrect login information provided.");
                }
            }
            else
            {
                if (!ViternusAuthenticationHelper.ValidateFacebookId(ref userName, facebookId))
                    ModelState.AddModelError("_FORM", "This facebook login has not been used to create an account on Viternus.");
            }

            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    return "An account for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return "An account with this Facebook information already exists. You can just try logging in with Facebook.";

                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        int MinRequiredNonAlphanumericCharacters { get; }

        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipCreateStatus CreateUser(string userName, string facebookId, string password, string email, bool agreedToUser);
        void UpdateUser(MembershipUser user);
        bool ValidateUser(string userName, string password);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? System.Web.Security.Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _provider.MinRequiredNonAlphanumericCharacters;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string facebookId, string password, string email, bool agreedToTOS)
        {
            MembershipCreateStatus status;
            if (String.IsNullOrEmpty(facebookId))
                ((L2EMembershipProvider)_provider).CreateUser(userName, password, email, null, null, true, null, out status, agreedToTOS);
            else
            {
                ((L2EMembershipProvider)_provider).CreateUser(userName, null, email, true, null, facebookId, out status, agreedToTOS);
            }
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }

        public void UpdateUser(MembershipUser user)
        {
            _provider.UpdateUser(user);
        }
    }
}
