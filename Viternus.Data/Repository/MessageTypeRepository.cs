using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class MessageTypeRepository : RepositoryBase<MessageType>, IMessageTypeRepository
    {
        protected override ObjectQuery<MessageType> EntityQuery
        {
            get { return DataConnector.Context.MessageType; }
        }
    }
}