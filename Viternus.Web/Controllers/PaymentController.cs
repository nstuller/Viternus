using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Viternus.Data;
using Viternus.Data.Repository;
using Viternus.Web.Filters;

namespace Viternus.Web.Controllers
{
    [LogError]
    public class PaymentController : ApplicationController
    {
        [Authorize]
        public ActionResult Card()
        {
            //If user has already been upgraded and is a premium user then don't send them to the payment page
            if (_roleRepository.IsUserInRole(User.Identity.Name, "Premium"))
                return RedirectToAction("Done");

            AppUser loggedInUser = _userRepository.GetByUserName(User.Identity.Name);

            //Here is where we redirect the user to Chargify's cc page
            StringBuilder queryString = new StringBuilder();
            queryString.Append("reference=");
            queryString.Append(loggedInUser.Id);
            queryString.Append("&email=");
            queryString.Append(loggedInUser.Profile.Email);
            queryString.Append("&first_name=");
            queryString.Append(loggedInUser.Profile.FirstName);
            queryString.Append("&last_name=");
            queryString.Append(loggedInUser.Profile.LastName);

            Response.Redirect(ConfigurationManager.AppSettings["ChargifyUrl"] + "?" + queryString.ToString());
            return View();
        }

        [Authorize]
        public ActionResult Done(int? subscriptionId, Guid? customerReference, string productHandle)
        {
            ViewData["ProgressStep"] = "2";

            if (null != customerReference)
            {
                //Do all the rules involved with a user upgrade
                //1. Set user as premium user
                AppUser userToUpgrade = _userRepository.GetById(customerReference.Value);
                Role premiumRole = _roleRepository.GetByRoleName("Premium");
                _roleRepository.AddUserToRole(userToUpgrade, premiumRole);
                _roleRepository.Save();
            }

            return View();
        }

        [Authorize(Roles = "Premium")]
        public ActionResult Redirect()
        {
            return RedirectToAction("NumTrustees", "InnerCircle");
        }
    }
}
