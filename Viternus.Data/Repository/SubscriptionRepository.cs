using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        protected override ObjectQuery<Subscription> EntityQuery
        {
            get { return DataConnector.Context.Subscription; }
        }
    }
}
