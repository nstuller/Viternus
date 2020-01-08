using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using Viternus.Data;
using Viternus.Data.Repository;


namespace Viternus.DeliveryAutomation
{
    public class DeliveryProcess
    {
        private MessageRepository _msgRepository = new MessageRepository();
        private AppUserRepository _userRepository = new AppUserRepository();
        private InnerCircleRepository _innerCircleRepository = new InnerCircleRepository();

        internal void Run()
        {
            MarkEventMessagesForDelivery();

            DeliverOverdueMessages();
        }

        public void MarkEventMessagesForDelivery()
        {
            //Another spot for dependency injection

            //1.  Select all messages that must be sent based on an event
            //Find all users who are trying to send a message based on an event happening
            var users = _userRepository.FindUsersWithEventBasedMessages();

            foreach (AppUser user in users)
            {
                //Set the death messages for delivery
                var deathMessages = user.Message.Where(m => null != m.EventType &&
                                                    m.EventType.Description == "Death").ToList(); ;

                //If the user has some death messages and we haven't already determined that he/she is deceased
                if (null == user.Profile.DeceasedDate && 0 < deathMessages.Count())
                {
                    //Schedule all the death messages to be delivered
                    foreach (Message msg in deathMessages)
                    {
                        msg.ScheduleDate = DateTime.Today.AddDays(Convert.ToDouble(msg.EventScheduleOffsetDays));
                    }
                }
                else
                {
                    //Zero results found -- not verified that this is where it will go
                }

                //Set the incapacitation messages
                var incapMessages = user.Message.Where(m => null != m.EventType &&
                                    m.EventType.Description == "Incapacitation").ToList(); ;

                if (null == user.Profile.IncapacitationDate && 0 < incapMessages.Count())
                {
                    //Schedule all the death messages to be delivered
                    foreach (Message msg in incapMessages)
                    {
                        msg.ScheduleDate = DateTime.Today.AddDays(Convert.ToDouble(msg.EventScheduleOffsetDays));
                    }
                }
                else
                {
                    //Zero results found -- not verified that this is where it will go
                }
            }

            _userRepository.Save();
        }

        public void DeliverOverdueMessages()
        {
            //2. Select all unsent where Date is in the past -- collects event messages too
            List<Message> messages = _msgRepository.FindAllOverdueMessages();

            //3. Deliver messages and mark as sent
            foreach (Message msg in messages)
            {
                //We've got all unsent messages with a past delivery date, so we probably need to send it

                //Go through the message recipients and create the e-mail
                foreach (MessageRecipient msgRecipient in msg.MessageRecipient)
                {
                    //This would be a great spot for dependency injection
                    //All we know is that we have to deliver the message here, we don't care what deliver means
                    MailMessage mailMsg = Emailer.GenerateMessageEmail(msgRecipient);
                    Emailer.SendEmail(mailMsg, msgRecipient.Id, EmailEntityType.MessageRecipient);
                }
            }

            _msgRepository.Save();
        }

        public void DeliverInnerCircleRequests()
        {
            //1. Select all unsent Inner Circle notifications/requests
            List<InnerCircle> chosenPeople = _innerCircleRepository.FindAllUnsentRecords();

            //2. Deliver notifications and mark as sent
            foreach (InnerCircle ic in chosenPeople)
            {
                //This would be a great spot for dependency injection
                MailMessage mailMsg = Emailer.GenerateInnerCircleRequestEmail(ic);
                Emailer.SendEmail(mailMsg, ic.Id, EmailEntityType.InnerCircle);

            }

            _innerCircleRepository.Save();
        }

        private static void SSDMFQuery(AppUser user)
        {
            #region Do Death Logic
            //TODO: Should I make the "Death" string less hard-coded?
            var deathMessages = user.Message.Where(m => null != m.EventType &&
                                                    m.EventType.Description == "Death").ToList(); ;

            //If the user has some death messages and we haven't already determined that he/she is deceased
            if (null == user.Profile.DeceasedDate && 0 < deathMessages.Count())
            {
                #region Web Service
                try
                {
                    SSDI.DMFwbsvcClass svc = new SSDI.DMFwbsvcClass();
                    svc.Timeout = 1200000;
                    //SummitQwest //VonWafer25
                    //SSN expects numbers with no dashes or spaces
                    //Dates Expect mm/dd/yyyy
                    //Maximum Rows likely expects number
                    DataSet ds = new DataSet();

                    ds = svc.GetQuery("SummitQwest", "VonWafer25", user.Profile.SSN, String.Empty, String.Empty,
                        String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "1000");
                    //ds.ReadXml(@"C:\FRecovery\Viternus\SSDIServiceResults-286789283.xml", XmlReadMode.ReadSchema);

                    //check for all the nulls and stuff
                    if (null != ds && 0 < ds.Tables.Count && 0 < ds.Tables["SSN"].Columns.Count)
                    {
                        if ("Error" == ds.Tables["SSN"].Columns[0].ColumnName && 0 < ds.Tables["SSN"].Rows.Count)
                        {
                            //Log how many rows there are, spit the whole dataset out, and highlight message
                            //ds.Tables["SSN"].Rows[0]["Error"]
                        }
                        else
                        {
                            if (0 < ds.Tables["SSN"].Rows.Count)
                            {
                                //TODO: Also should double check to make sure that the web service responded with the right person

                                //Find the deceased date from the response feed

                                user.Profile.DeceasedDate =
                                    DateTime.ParseExact(ds.Tables["SSN"].Rows[0]["DOD"].ToString(), "MMddyyyy", CultureInfo.InvariantCulture);

                                //Schedule all the death messages to be delivered
                                foreach (Message msg in deathMessages)
                                {
                                    msg.ScheduleDate = DateTime.Today.AddDays(Convert.ToDouble(msg.EventScheduleOffsetDays));
                                }
                            }
                            else
                            {
                                //Zero results found -- not verified that this is where it will go
                            }
                        }
                    }
                }
                catch (System.ServiceModel.FaultException)
                {
                    //Gotta log here too
                }
                #endregion
            }

            #endregion
        }
    }
}
