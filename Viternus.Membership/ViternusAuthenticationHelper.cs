using System.Configuration;
using FBConnectAuth;
using Viternus.Data;
using Viternus.Data.Repository;

namespace Viternus.Membership
{
    public class ViternusAuthenticationHelper
    {
        public static bool ValidateFacebookId(ref string username, string facebookId)
        {
            string apiKey = ConfigurationManager.AppSettings["FacebookAPIKey"] as string;
            string apiSecret = ConfigurationManager.AppSettings["FacebookApplicationSecret"] as string;

            FBConnectAuthentication auth = new FBConnectAuthentication(apiKey, apiSecret); 
            if (auth.Validate() == ValidationState.Valid)
            {
                FBConnectSession fbSession = auth.GetSession();
                string userId = fbSession.UserID;

                AppUserRepository userRepository = new AppUserRepository();
                AppUser user = userRepository.GetByFacebookId(userId);

                if (null != user)
                {
                    username = user.UserName;
                    return true;
                }
            }
            return false;
        }
    }
}
