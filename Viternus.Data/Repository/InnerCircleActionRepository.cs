using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class InnerCircleActionRepository : RepositoryBase<InnerCircleAction>, IInnerCircleActionRepository
    {
        protected override ObjectQuery<InnerCircleAction> EntityQuery
        {
            get { return DataConnector.Context.InnerCircleAction; }
        }

        public void AddInnerCircleActionToEventType(InnerCircleAction newInnerCircleAction, Guid eventTypeId)
        {
            EventTypeRepository etRep = new EventTypeRepository();
            etRep.GetById(eventTypeId).InnerCircleAction.Add(newInnerCircleAction);
        }

        public void AddInnerCircleActionToInnerCircle(InnerCircleAction newInnerCircleAction, InnerCircle ic)
        {
            ic.InnerCircleAction.Add(newInnerCircleAction);
        }

        public InnerCircleAction GetByInnerCircleAndUserNameAndEventTypeId(InnerCircle ic, string userName, Guid eventType)
        {
            var result = from ica in DataConnector.Context.InnerCircleAction.Include("InnerCircle.AppUser.Profile").Include("InnerCircle.Profile.AppUser")
                         where ica.InnerCircle.Id == ic.Id 
                         && ica.InnerCircle.Profile.AppUser.FirstOrDefault().UserName == userName
                         && ica.EventType.Id == eventType
                         select ica;

            return result.FirstOrDefault();
        }

        public InnerCircleAction GetByInnerCircleAndUserNameAndEventTypeDesc(InnerCircle ic, string userName, string eventType)
        {
            var result = from ica in DataConnector.Context.InnerCircleAction.Include("InnerCircle.AppUser.Profile").Include("InnerCircle.Profile.AppUser")
                         where ica.InnerCircle.Id == ic.Id
                         && ica.InnerCircle.Profile.AppUser.FirstOrDefault().UserName == userName
                         && ica.EventType.Description == eventType
                         select ica;

            return result.FirstOrDefault();
        }
    }
}
