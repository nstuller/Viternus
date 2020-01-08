using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
    {
        protected override ObjectQuery<Profile> EntityQuery
        {
            get { return DataConnector.Context.Profile; }
        }

        public Profile GetByEmail(string email)
        {
            var result = from p in DataConnector.Context.Profile.Include("AppUser").Include("MessageRecipient.Message")
                         where p.Email == email
                         select p;

            return result.FirstOrDefault();
        }

        public Profile GetByFacebookId(string fbId)
        {
            var result = from p in DataConnector.Context.Profile.Include("AppUser").Include("MessageRecipient.Message")
                         where p.FacebookId == fbId
                         select p;

            return result.FirstOrDefault();
        }

        public Profile New(string firstName, string lastName, string nickname, string email)
        {
            Profile profile = New();

            profile.FirstName = firstName;
            profile.LastName = lastName;
            profile.NickName = nickname;
            profile.Email = email;

            DataConnector.Context.AddToProfile(profile);
            return profile;
        }

        public void AddUserToProfile(AppUser user, Profile prof)
        {
            prof.AppUser.Add(user);
        }
    }
}
