using System.Web.Mvc;
using Viternus.Data;
using Viternus.Data.Repository;

namespace Viternus.Web.Controllers
{
    public abstract class ApplicationController : Controller
    {
        protected AppUserRepository _userRepository = new AppUserRepository();
        protected RoleRepository _roleRepository = new RoleRepository();
        protected JournalPromptRepository _journalRepository = new JournalPromptRepository();

        public string JournalPromptText
        {
            get 
            {
                if (null != Session["journalPrompt"])
                    return (string)Session["journalPrompt"];
                else
                {
                    string txt = _journalRepository.GetRandom().Text;
                    Session["journalPrompt"] = txt;
                    return txt;
                }
            }
        }
        
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            //TODO: There is hopefully a way to retrieve/cache the user without having to go back to the database every time
            AppUser user = _userRepository.GetByUserName(User.Identity.Name);

            if (null != user)
            {
                ViewData["showChangePassword"] = user.CanChangePassword;
                //ViewData["showUpgrade"] = !User.IsInRole("Premium");
                //TODO:  May want to create a custom principal object so I can make the above call
                ViewData["showUpgrade"] = !_roleRepository.IsUserInRole(user.UserName, "Premium");
                ViewData["showMyInnerCircle"] = !(bool)ViewData["showUpgrade"]; //They should have access to Inner Circle if they are premium
                ViewData["showInnerCircleMember"] = user.IsInnerCircleMember;
            }

            ViewData["showAds"] = (null != user ? user.DisplayAds : true);
            ViewData["journalPrompt"] = JournalPromptText;
        }
    }
}
