using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        protected override ObjectQuery<AppUser> EntityQuery
        {
            get { return DataConnector.Context.AppUser; }
        }

        /// <summary>
        /// Find all users who are trying to send a message based on an event happening
        /// </summary>
        /// <returns></returns>
        public List<AppUser> FindUsersWithEventBasedMessages()
        {
            var results = from a in DataConnector.Context.AppUser.Include("Message.EventType").Include("Profile")
                          where a.Message.Any(c => c.EventType != null)
                          select a;

            return results.ToList();
        }

        public AppUser GetByUserName(string userName)
        {
            var results = FindByUserName(userName);

            return results.FirstOrDefault();
        }

        public List<AppUser> FindByUserName(string userName)
        {
            var results = from a in EntityQuery.Include("Profile.InnerCircle.AppUser.Profile").Include("Profile.InnerCircle.InnerCircleAction.EventType").Include("Video").Include("InnerCircle.Profile")
                          where a.UserName.ToLower().Contains(userName.ToLower())
                          select a;

            return results.ToList();
        }

        public AppUser GetByEmail(string email)
        {
            var results = FindByEmail(email);

            return results.FirstOrDefault();
        }

        public List<AppUser> FindByEmail(string email)
        {
            var results = from a in EntityQuery.Include("Profile").Include("Video")
                          where a.Profile.Email.ToLower() == email.ToLower()
                          select a;

            return results.ToList();
        }

        public AppUser GetByFacebookId(string facebookId)
        {
            var results = from a in EntityQuery.Include("Profile")
                          where a.Profile.FacebookId.ToLower().Equals(facebookId.ToLower())
                          select a;

            return results.FirstOrDefault();
        }
    }
}
