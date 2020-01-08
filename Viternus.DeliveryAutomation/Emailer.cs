using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viternus.Data;
using System.Net.Mail;
using System.ComponentModel;
using Viternus.Data.Repository;
using System.Threading;

namespace Viternus.DeliveryAutomation
{
    public enum EmailEntityType
    {
        None,
        MessageRecipient,
        InnerCircle
    }

    public class Emailer
    {
        internal static MailMessage GenerateMessageEmail(MessageRecipient msgRecip)
        {
            Console.WriteLine(String.Format("Sending an e-mail to {0} at {1}{2}", msgRecip.Profile.FirstName, msgRecip.Profile.Email, Environment.NewLine));

            try
            {
                MailMessage mail = new MailMessage();

                string firstName = msgRecip.Message.AppUser.Profile.FirstName;
                string lastName = msgRecip.Message.AppUser.Profile.LastName;
                if (!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
                    mail.From = new MailAddress("MessageDelivery@Viternus.com", string.Format("{0} {1}", firstName, lastName));

                if (!string.IsNullOrEmpty(msgRecip.Profile.Email))
                    mail.To.Add(msgRecip.Profile.Email);
                if (!string.IsNullOrEmpty(msgRecip.Profile.EmailAlternate))
                    mail.To.Add(msgRecip.Profile.EmailAlternate);
                mail.Subject = "You Have a Viternus Message Waiting for You";
                mail.IsBodyHtml = false;

                StringBuilder body = new StringBuilder();
                body.Append("Dear ");
                body.Append(String.IsNullOrEmpty(msgRecip.Profile.FirstName) ? "Recipient" : msgRecip.Profile.FirstName);
                body.Append(",");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("A message awaits you on the Viternus website, at the link below:");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("(Click the link to view the message)");
                body.Append(Environment.NewLine);
                body.Append(@"http://www.Viternus.com/Message/ViewMessageFromUrl/" + msgRecip.Id);
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                body.Append("This message has been sent by ");
                if (String.IsNullOrEmpty(firstName) && String.IsNullOrEmpty(lastName))
                {
                    body.Append(msgRecip.Message.AppUser.UserName);
                }
                else
                {
                    body.Append(firstName);
                    body.Append(" ");
                    body.Append(lastName);
                }
                body.Append(".  The message is not urgent, but he/she requests you view it at your earliest convenience.  If you are not ");
                if (String.IsNullOrEmpty(msgRecip.Profile.FirstName) && String.IsNullOrEmpty(msgRecip.Profile.LastName))
                {
                    body.Append("the owner of this email address");
                }
                else
                {
                    body.Append(msgRecip.Profile.FirstName);
                    body.Append(" ");
                    body.Append(msgRecip.Profile.LastName);
                }
                body.Append(", then please disregard this message.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("Thank you,");
                body.Append(Environment.NewLine);
                body.Append("Viternus Webmaster");

                mail.Body = body.ToString();
                return mail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                ErrorLogRepository errorRepos = new ErrorLogRepository();
                errorRepos.SaveErrorToDB(ex, "GenerateMessageMail failed", String.Empty);
                return null;
            }
        }

        internal static MailMessage GenerateInnerCircleRequestEmail(InnerCircle trustee)
        {
            Console.WriteLine(String.Format("Sending an e-mail to {0} at {1}{2}", trustee.Profile.FirstName, trustee.Profile.Email, Environment.NewLine));

            try
            {
                MailMessage mail = new MailMessage();

                string firstName = trustee.AppUser.Profile.FirstName;
                string lastName = trustee.AppUser.Profile.LastName;
                if (!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
                    mail.From = new MailAddress("MessageDelivery@Viternus.com", string.Format("{0} {1}", firstName, lastName));

                if (!string.IsNullOrEmpty(trustee.Profile.Email))
                    mail.To.Add(trustee.Profile.Email);
                if (!string.IsNullOrEmpty(trustee.Profile.EmailAlternate))
                    mail.To.Add(trustee.Profile.EmailAlternate);
                mail.Subject = "You have been designated as a trustee";
                mail.IsBodyHtml = false;

                StringBuilder body = new StringBuilder();
                body.Append("Dear ");
                body.Append(String.IsNullOrEmpty(trustee.Profile.FirstName) ? "Trustee" : trustee.Profile.FirstName);
                body.Append(",");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append(firstName);
                body.Append(" ");
                body.Append(lastName);
                body.Append(" listed you as a Trustee in the Inner Circle.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("With this designation comes the following responsibility:");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("- Immediate Action Required: Create your free account on Viternus.com");
                body.Append(Environment.NewLine);
                body.Append("- Immediate Action Required: Accept the responsiblity of being a trustee");
                body.Append(Environment.NewLine);
                body.Append("- Future Action Required: Should something happen to " + firstName + ", such as incapacitation or death, log in and notify Viternus via the 'I am a Trustee' link.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                //TODO: Right now we always assume the Inner Circle member needs a new account
                body.Append("To fulfill your responsibility, click the link below and create your account:");
                body.Append(Environment.NewLine);
                body.Append(@"http://www.Viternus.com/Account/Register?email=" + trustee.Profile.Email + "&userName=" + trustee.Profile.ProposedUserName + "&ReturnUrl=%2fInnerCircle%2fMember");

                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);
                body.Append("Thank you,");
                body.Append(Environment.NewLine);
                body.Append("Viternus Webmaster");


                mail.Body = body.ToString();
                return mail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                ErrorLogRepository errorRepos = new ErrorLogRepository();
                errorRepos.SaveErrorToDB(ex, "GenerateInnerCircleRequestEmail failed", String.Empty);
                return null;
            }
        }

        public static void SendEmail(MailMessage mail, object token, EmailEntityType typeToSave)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = false;
                smtp.Send(mail);
                Console.WriteLine("Completed MessageRecipient.Id:" + token);

                if (null != token)
                {
                    if (EmailEntityType.MessageRecipient == typeToSave)
                    {
                        Guid messageRecipientId = new Guid(token.ToString());
                        MessageRepository msgRepository = new MessageRepository();
                        Message msg = msgRepository.FindByMessageRecipientId(messageRecipientId);
                        msg.SentDate = DateTime.Now;
                        msgRepository.Save(msg);
                    }
                    else if (EmailEntityType.InnerCircle == typeToSave)
                    {
                        Guid innerCircleId = new Guid(token.ToString());
                        InnerCircleRepository icRepository = new InnerCircleRepository();
                        InnerCircle ic = icRepository.GetById(innerCircleId);
                        ic.NotificationSentDate = DateTime.Now;
                        icRepository.Save(ic);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }


    }
}
