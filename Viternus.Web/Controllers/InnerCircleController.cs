using System;
using System.Web.Mvc;
using Viternus.Data;
using Viternus.Data.Repository;
using Viternus.DeliveryAutomation;
using Viternus.Web.ViewModels;
using Viternus.Web.Filters;

namespace Viternus.Web.Controllers
{
    [LogError]
    public class InnerCircleController : ApplicationController
    {
        private ProfileRepository _profileRepository = new ProfileRepository();
        private InnerCircleRepository _innerCircleRepository = new InnerCircleRepository();
        private InnerCircleActionRepository _innerCircleActionRepository = new InnerCircleActionRepository();


        public ActionResult DeliverOutstandingRequests(bool? scheduled)
        {
            //This is an admin function but we need to be able to automatically schedule it
            bool schedParam = scheduled ?? false;
            if (User.IsInRole("Admin") || schedParam)
            {
                DeliveryProcess dp = new DeliveryProcess();
                dp.DeliverInnerCircleRequests();
                return Content("Success");
            }

            return Content("Not Authorized");
        }

        [Authorize]
        public ActionResult Explain()
        {
            ViewData["ProgressStep"] = "1";
            return View();
        }

        [Authorize(Roles = "Premium")]
        public ActionResult NumTrustees()
        {
            ViewData["ProgressStep"] = "3";
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);
            return View(new InnerCircleNumTrusteesViewModel(user));
        }

        [Authorize(Roles = "Premium")]
        public ActionResult Choose()
        {
            ViewData["ProgressStep"] = "4";
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);
            string numTrustees = Request.QueryString["NumTrustees"];
            if (!String.IsNullOrEmpty(numTrustees))
            {
                user.Profile.NumberOfTrustees = short.Parse(numTrustees);
                _userRepository.Save();
            }

            return View(new InnerCircleChooseViewModel(user));
        }

        [Authorize(Roles = "Premium")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Choose(string email, string firstName, string lastName)
        {
            ViewData["ProgressStep"] = "4";
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);
            //By the time we get here, we should be able to assume that the user has chosen how many in their IC
            short numTrustees = user.Profile.NumberOfTrustees.Value;

            if (numTrustees <= user.InnerCircle.Count)
            {
                ModelState.AddModelError("_FORM", "You have chosen to have " + numTrustees.ToString() + " member(s) in your Inner Circle. There is no need to add any more.");
            }
            else if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "Please enter an email address.  It is required.");
            }
            else
            {
                bool hasError = false;
                Profile profile = _profileRepository.GetByEmail(email);
                InnerCircle newInnerCircle = _innerCircleRepository.GetByEmailAndUserId(email, user.Id);

                if (null == profile)
                {
                    if (string.IsNullOrEmpty(firstName))
                    {
                        ModelState.AddModelError("firstName", "Please enter a first name.  It is required when no profile matches the email address.");
                        hasError = true;
                    }
                    if (string.IsNullOrEmpty(lastName))
                    {
                        ModelState.AddModelError("lastName", "Please enter a last name.  It is required when no profile matches the email address.");
                        hasError = true;
                    }

                    profile = _profileRepository.New(firstName, lastName, String.Empty, email);
                }

                if (null == newInnerCircle)
                {
                    newInnerCircle = _innerCircleRepository.New();
                }

                if (!hasError)
                {
                    _innerCircleRepository.AddInnerCircleToProfile(newInnerCircle, profile);
                    _innerCircleRepository.AddInnerCircleToUser(newInnerCircle, user);
                    _innerCircleRepository.Save();
                }
            }

            return View(new InnerCircleChooseViewModel(user));
        }

        [Authorize(Roles = "Premium")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Guid id)
        {
            InnerCircle chosenOne = null;
            try
            {
                chosenOne = _innerCircleRepository.GetById(id);

                if (chosenOne == null)
                    return View("NotFound");

                _innerCircleRepository.Delete(chosenOne);

                return RedirectToAction("Choose");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize(Roles = "Premium")]
        public ActionResult Done()
        {
            ViewData["ProgressStep"] = "5";

            //Since user completed the whole Inner Circle process, go ahead and deliver requests
            DeliverOutstandingRequests(true);

            return View();
        }

        [Authorize]
        public ActionResult Member()
        {
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);

            return View(new InnerCircleMemberViewModel(user));
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Accept(Guid id)
        {
            InnerCircle ic = _innerCircleRepository.GetById(id); 
            //Can never turn this off (client-side script disables the checkbox but it could be hacked
            ic.AcceptedResponsibility = true; // !ic.AcceptedResponsibility;
            _innerCircleRepository.Save();

            return RedirectToAction("Member");
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LifeEvent(Guid innerCircleId, Guid eventType, DateTime eventDate)
        {
            InnerCircle ic = _innerCircleRepository.GetById(innerCircleId);

            //TODO: Business Rules should be relocated
            InnerCircleAction existingIca = _innerCircleActionRepository.GetByInnerCircleAndUserNameAndEventTypeId(ic, User.Identity.Name, eventType);
            if (null != existingIca)
            {
                ModelState.AddModelError("_FORM", "Thanks, but we already have a record of you notifying us of this event. You notified us on " + String.Format("{0:d}", existingIca.ActionDate));
                AppUser user = _userRepository.GetByUserName(User.Identity.Name);
                return View("Member", new InnerCircleMemberViewModel(user));
            }

            //if a trustee is notifying viternus of a life event, then he/she has accepted responsibility
            ic.AcceptedResponsibility = true;

            InnerCircleAction ica = _innerCircleActionRepository.New();
            _innerCircleActionRepository.AddInnerCircleActionToEventType(ica, eventType);
            ica.EventDate = eventDate;
            ica.ActionDate = DateTime.Now;
            _innerCircleActionRepository.AddInnerCircleActionToInnerCircle(ica, ic);

            //TODO: Why am I just using the last entered Event Date?
            if ("Incapacitation" == ica.EventType.Description && ic.MajorityIncapacitated)
                ic.AppUser.Profile.IncapacitationDate = eventDate;
            else if ("Death" == ica.EventType.Description && ic.MajorityDeceased)
                ic.AppUser.Profile.DeceasedDate = eventDate;
    
            _innerCircleActionRepository.Save();

            return RedirectToAction("Member");
        }
    }
}
