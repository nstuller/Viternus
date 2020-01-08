using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.Data.Repository;
using System.Web.Mvc;
using Viternus.Data;

namespace Viternus.Web.ViewModels
{
    public class MessageDisplayViewModel
    {
        // Properties
        public Message Message { get; private set; }
        public SelectList MessageTypes { get; private set; }
        public SelectList EventTypes { get; private set; }
        public SelectList MyVideos { get; private set; }
        public AppUser User { get; private set; }

        // Constructor
        public MessageDisplayViewModel(Message message, string userName)
        {
            Message = message;

            MessageTypeRepository mtRep = new MessageTypeRepository();
            if (null == Message.MessageTypeReference.EntityKey)
                MessageTypes = new SelectList(mtRep.GetAll(), "Id", "Description");
            else
                MessageTypes = new SelectList(mtRep.GetAll(), "Id", "Description", Message.MessageTypeReference.EntityKey.EntityKeyValues[0].Value);

            EventTypeRepository etRep = new EventTypeRepository();
            if (null == Message.EventTypeReference.EntityKey)
                EventTypes = new SelectList(etRep.GetAll(), "Id", "Description");
            else
                EventTypes = new SelectList(etRep.GetAll(), "Id", "Description", Message.EventTypeReference.EntityKey.EntityKeyValues[0].Value);

            VideoRepository vRep = new VideoRepository();
            if (null == Message.VideoReference.EntityKey)
                MyVideos = new SelectList(vRep.FindAllVideosByUser(userName).ToList(), "Id", "Description");
            else
                MyVideos = new SelectList(vRep.FindAllVideosByUser(userName).ToList(), "Id", "Description", Message.VideoReference.EntityKey.EntityKeyValues[0].Value);

            if (!string.IsNullOrEmpty(userName))
            {
                AppUserRepository userRepository = new AppUserRepository();
                User = userRepository.GetByUserName(userName);
            }
        }
    }
}
