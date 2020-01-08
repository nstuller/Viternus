using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class InnerCircleRepository : RepositoryBase<InnerCircle>, IInnerCircleRepository
    {
        protected override ObjectQuery<InnerCircle> EntityQuery
        {
            get { return DataConnector.Context.InnerCircle; }
        }

        //TEMPORARY: Can remove this method when EF 4 is used and deferred lazy loading is an option
        //We need this method so that we can explicitly load references, (this might involve more db trips)
        public override InnerCircle GetById(Guid id)
        {
            var result = from m in DataConnector.Context.InnerCircle.Include("Profile.AppUser").Include("AppUser.Profile").Include("InnerCircleAction")
                         where m.Id == id
                         select m;

            return result.FirstOrDefault();
        }


        public InnerCircle GetByEmailAndUserId(string email, Guid userId)
        {
            var result = from ic in DataConnector.Context.InnerCircle.Include("Profile.AppUser").Include("AppUser.Profile").Include("InnerCircleAction")
                         where ic.Profile.Email == email && ic.AppUser.Id == userId
                         select ic;

            return result.FirstOrDefault();
        }

        public void AddInnerCircleToProfile(InnerCircle newInnerCircle, Profile profile)
        {
            profile.InnerCircle.Add(newInnerCircle);
        }       

        public void AddInnerCircleToUser(InnerCircle newInnerCircle, AppUser user)
        {
            user.InnerCircle.Add(newInnerCircle);
        }

        public List<InnerCircle> FindAllUnsentRecords()
        {
            var result = from ic in DataConnector.Context.InnerCircle.Include("Profile.AppUser").Include("AppUser.Profile")
                         where null == ic.NotificationSentDate
                         select ic;

            return result.ToList();
        }
    }
}
