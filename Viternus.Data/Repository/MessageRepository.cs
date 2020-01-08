using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        protected override ObjectQuery<Message> EntityQuery
        {
            get { return DataConnector.Context.Message; }
        }

        //TEMPORARY: Can remove this method when EF 4 is used and deferred lazy loading is an option
        //We need this method so that we can explicitly load references, (this might involve more db trips)
        public override Message GetById(Guid rowId)
        {
            var results = from e in EntityQuery.Include("MessageRecipient.Profile").Include("AppUser.Profile")
                          where e.Id == rowId
                          select e;

            return results.FirstOrDefault();    
        }

        public List<Message> FindAllMessagesByFromUser(string userName)
        {
            var result = from m in DataConnector.Context.Message.Include("Video").Include("MessageType").Include("AppUser.Profile").Include("MessageRecipient.Profile")
                         where m.AppUser.UserName == userName
                         orderby m.ScheduleDate ascending
                         select m;

            return result.ToList();
        }

        public List<Message> FindAllOverdueMessages()
        {
            var result = from m in DataConnector.Context.Message.Include("MessageRecipient.Profile").Include("AppUser.Profile")
                           where null == m.SentDate
                            && (m.ScheduleDate < DateTime.Now)
                           select m;

            return result.ToList();
        }
        
        public Message FindByMessageRecipientId(Guid messageRecipientId)
        {
            var result = from m in DataConnector.Context.MessageRecipient.Include("Message.Video").Include("Message.MessageType").Include("Message.AppUser.Profile").Include("Profile")
                         where m.Id == messageRecipientId
                         select m;

            return result.First().Message;
        }

        public void AddMessageToMessageType(Message msg, Guid messageTypeId)
        {
            MessageTypeRepository mtRep = new MessageTypeRepository();
            mtRep.GetById(messageTypeId).Message.Add(msg);
        }

        public void AddMessageToEventType(Message msg, Guid eventTypeId)
        {
            EventTypeRepository etRep = new EventTypeRepository();
            etRep.GetById(eventTypeId).Message.Add(msg);
        }

        public void AddMessageToVideo(Message msg, Guid videoId)
        {
            VideoRepository vRep = new VideoRepository();
            vRep.GetById(videoId).Message.Add(msg);
        }

        public void AddMessageToUser(Message msg, string userName)
        {
            AppUserRepository userRepository = new AppUserRepository();
            userRepository.GetByUserName(userName).Message.Add(msg);
        }
    }
}
