using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.Data.Repository;
using System.Web.Mvc;
using Viternus.Data;

namespace Viternus.Web.ViewModels
{
    public class MessageRecipientDisplayViewModel : MessageDisplayViewModel
    {
        public MessageRecipient MessageRecipient { get; private set; }

        public bool RecipientAlreadyIsAUser
        {
            get
            {
                AppUser recipientUser = MessageRecipient.Profile.AppUser.FirstOrDefault();
                //Return true if the recipient is already a user or if any user is already logged in
                return (null != recipientUser || HttpContext.Current.User.Identity.IsAuthenticated);
            }
        }

        public string FirstName { get { return MessageRecipient.Profile.FirstName; } }
        public string LastName { get { return MessageRecipient.Profile.LastName; } }
        public string ProposedUserName { get { return MessageRecipient.Profile.ProposedUserName; } }


        // Constructor
        public MessageRecipientDisplayViewModel(MessageRecipient msgRecip, string userName)
            : base(msgRecip.Message, userName)
        {
            MessageRecipient = msgRecip;
        }
    }
}
