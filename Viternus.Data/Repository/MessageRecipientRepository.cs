using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;
using System.Data.Objects;

namespace Viternus.Data.Repository
{
    public class MessageRecipientRepository : RepositoryBase<MessageRecipient>, IMessageRecipientRepository
    {
        protected override ObjectQuery<MessageRecipient> EntityQuery
        {
            get { return DataConnector.Context.MessageRecipient; }
        }

        //TEMPORARY: Can remove this method when EF 4 is used and deferred lazy loading is an option
        //We need this method so that we can explicitly load references, (this might involve more db trips)
        public override MessageRecipient GetById(Guid id)
        {
            var result = from m in DataConnector.Context.MessageRecipient.Include("Message.Video").Include("Profile.AppUser").Include("Message.AppUser.Profile")
                         where m.Id == id
                         select m;

            return result.FirstOrDefault();
        }

        //This should return a collection
        public MessageRecipient GetByMessageId(Guid id)
        {
            var result = from m in DataConnector.Context.MessageRecipient.Include("Message").Include("Profile")
                         where m.Message.Id == id
                         select m;

            return result.FirstOrDefault();
        }

        public MessageRecipient GetByEmailAndMessageId(string emailAddress, Guid messageId)
        {
            var result = from m in DataConnector.Context.MessageRecipient.Include("Message").Include("Profile")
                         where m.Profile.Email == emailAddress && m.Message.Id == messageId
                         select m;

            return result.FirstOrDefault();
        }

        public MessageRecipient GetByIdIfNotSent(Guid id)
        {
            //Added security measure: Do not display a video unless it has been delivered
            var result = from m in DataConnector.Context.MessageRecipient.Include("Message.Video").Include("Profile.AppUser").Include("Message.AppUser.Profile")
                         where m.Id == id
                            &&  null != m.Message.SentDate
                         select m;

            return result.FirstOrDefault();
        }
        
        public void AddMessageRecipientToMessage(MessageRecipient msgRecipient, Message msg)
        {
            msg.MessageRecipient.Add(msgRecipient);
        }

        public void AddMessageRecipientToProfile(MessageRecipient msgRecipient, Profile prof)
        {
            prof.MessageRecipient.Add(msgRecipient);
        }
    }
}
