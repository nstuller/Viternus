using Viternus.Data.Interface;
using Viternus.Data.Repository;

namespace Viternus.Data
{
    public partial class AppUser : IEntity
    {
        public bool CanChangePassword
        {
            get
            {
                return (null == this.Profile.FacebookId);
            }
        }

        public bool DisplayAds
        {
            get
            {
                //If user has paid something then no ads
                return !UserIsAPaidUser;
            }
        }

        public bool IsInnerCircleMember
        {
            get
            {
                return this.Profile.InnerCircle.Count > 0;
            }
        }

        public bool LimitVideoUploads
        {
            get
            {
                //If user has paid something then limit video uploads
                return !UserIsAPaidUser;
            }
        }

        public int RemainingVideoUploads
        {
            get
            {
                if (!LimitVideoUploads)
                    return 10; //return int.MaxValue;
                else
                    return (2 - this.Video.Count);
            }
        }

        public bool UserIsAPaidUser
        {
            get
            {
                //If user is in the role Premium or Basic (or whatever we call them)
                RoleRepository roleRep = new RoleRepository();
                return (roleRep.IsUserInRole(this.UserName, "Premium") || roleRep.IsUserInRole(this.UserName, "Basic"));
            }
        }
    }
}
