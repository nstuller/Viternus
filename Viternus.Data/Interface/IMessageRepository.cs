using System;
using System.Linq;
using System.Collections.Generic;

namespace Viternus.Data.Interface
{
    interface IMessageRepository : IRepository<Message>
    {
        void AddMessageToMessageType(Message msg, Guid messageTypeId);
        void AddMessageToUser(Message msg, string userName);
        void AddMessageToVideo(Message msg, Guid videoId);
        List<Message> FindAllMessagesByFromUser(string userName);
        List<Message> FindAllOverdueMessages();
        Message FindByMessageRecipientId(Guid messageRecipientId);
    }
}
