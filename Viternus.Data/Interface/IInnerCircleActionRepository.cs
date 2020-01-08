using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viternus.Data.Interface
{
    interface IInnerCircleActionRepository : IRepository<InnerCircleAction>
    {
        void AddInnerCircleActionToEventType(InnerCircleAction newInnerCircleAction, Guid eventTypeId);
        void AddInnerCircleActionToInnerCircle(InnerCircleAction newInnerCircleAction, InnerCircle ic); 
        InnerCircleAction GetByInnerCircleAndUserNameAndEventTypeId(InnerCircle ic, string userName, Guid eventType);
        InnerCircleAction GetByInnerCircleAndUserNameAndEventTypeDesc(InnerCircle ic, string userName, string eventType);
    }
}
