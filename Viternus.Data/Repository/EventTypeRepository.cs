using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class EventTypeRepository : RepositoryBase<EventType>, IEventTypeRepository
    {
        protected override ObjectQuery<EventType> EntityQuery
        {
            get { return DataConnector.Context.EventType; }
        }
    }
}