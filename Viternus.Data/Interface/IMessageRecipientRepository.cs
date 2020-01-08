using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viternus.Data.Interface
{
    interface IMessageRecipientRepository : IRepository<MessageRecipient>
    {
        MessageRecipient GetByMessageId(Guid id);
        void AddMessageRecipientToMessage(MessageRecipient msgRecipient, Message msg);
        void AddMessageRecipientToProfile(MessageRecipient msgRecipient, Profile prof);
    }
}
