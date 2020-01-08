using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Viternus.Data;
using Viternus.Data.Repository;
using Viternus.Web.Filters;

namespace Viternus.Web.Controllers
{
    [LogError]
    public class HomeController : ApplicationController
    {
        private SubscriptionRepository _subscriptionRepository = new SubscriptionRepository();
        private NotificationRepository _notificationRepository = new NotificationRepository();

        //
        // GET: /Message/

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            else
                return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsOfService()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactUs(string name, string email, string message)
        {
            MailMessage mailItem = new MailMessage();
            mailItem.To.Add("Hello@Viternus.com");
            mailItem.From = new MailAddress(String.IsNullOrEmpty(email) ? "MessageDelivery@Viternus.com" : email, name);
            mailItem.Subject = "Feedback from Website";
            mailItem.Body = message;

            Viternus.DeliveryAutomation.Emailer.SendEmail(mailItem, null, Viternus.DeliveryAutomation.EmailEntityType.None);

            //Give some feedback to user that it's done
            return View("ContactUs", (object)"Message Delivered.");
        }

        public ActionResult Subscribe()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Subscribe(string name, string email, string message)
        {
            Subscription sub = _subscriptionRepository.New();
            sub.ProspectName = name;
            sub.Email = email;
            //For now, arbitrarily chop off the end. Only can hold 500 chars in DB
            sub.Feedback = string.IsNullOrEmpty(message) ? null : message.Substring(0, 500);

            //Write to database
            _subscriptionRepository.Save(sub);

            //Give some feedback to user that it's done
            return View("Subscribe", (object)"You have successfully subscribed to updates.");
        }

        public ActionResult VideoWillsWhitePaper()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VideoWillsWhitePaper(string email)
        {
            try
            {
                //TODO: Should refactor this or remove it later (after I use MailChimp or something)...
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("MessageDelivery@Viternus.com", "Viternus White Paper");

                //if (!string.IsNullOrEmpty(email))
                mail.To.Add(email);
                mail.Subject = "White Paper: Effectively Communicate Your Last Desires with a Video Will";
                mail.IsBodyHtml = true;



                Subscription sub = _subscriptionRepository.New();
                //sub.ProspectName = name;
                sub.Email = email;
                sub.Feedback = "Video Will White Paper";

                //Write to database
                _subscriptionRepository.Save(sub);

                

                System.Text.StringBuilder body = new System.Text.StringBuilder();
                body.Append("Greetings,<br /><br />");
                body.Append("We are very pleased that you have requested our white paper:<br /><br />");
                body.Append("<b>Effectively Communicate Your Last Desires with a Video Will</b><br /><br />");
                body.Append("Attached to this e-mail is a (pdf) file containing valuable information about video wills.<br /><br /><br />");
                body.Append("In the future, we will continue to reach out to you with the most recent news and ");
                body.Append("tips about Viternus in addition to delivering some great special offers!<br /><br />");
                body.Append("As always, please remember that if you have any questions we're only an email away.<br /><br /><br />");
                body.Append("Enjoy,<br />");
                body.Append("Nathan<br /><br />");
                body.Append("Nathan Stuller<br />");
                body.Append("President<br />");
                body.Append("Email: <a href='mailto:hello@viternus.com'>hello@viternus.com</a><br />");
                body.Append("<a href='http://www.viternus.com'>http://www.viternus.com</a><br />");
                mail.Body = body.ToString();

                Attachment whitePaperPdf = new Attachment(System.IO.Path.Combine(HttpContext.Server.MapPath("../Upload"), "White Paper - Six Reasons Why Video Wills Are Superior to Paper.pdf"));
                mail.Attachments.Add(whitePaperPdf);
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = false;
                smtp.Send(mail);
                //Done e-mailing

                //Give some feedback to user that it's done
                return View("VideoWillsWhitePaper", (object)"Your White Paper will be delivered to your email address shortly.<br/><br/>If you believe others can benefit from this information, please share <a href='/Home/VideoWillsWhitePaper'>our website!</a>");
            }
            catch (Exception ex)
            {
                //Give some feedback to user that it's done
                return View("VideoWillsWhitePaper", (object)"There was an error delivering to your email address.  Please try another address that is similar to the example 'yourname@example.com'");
            }
        }

        [Authorize()]
        public ActionResult Dashboard()
        {
            return View();
        }



        public class JsonNotification
        {
            public string Message { get; set; }

            public JsonNotification(string msg)
            {
                Message = msg;
            }
        }

        [Authorize()]
        public JsonResult GetNotifications()
        {
            //Get newest Video notifications
            List<Notification> newNotifications = _notificationRepository.FindAllNewNotificationsByUser(User.Identity.Name);

            if (0 < newNotifications.Count)
            {
                List<JsonNotification> results = new List<JsonNotification>();

                foreach (Notification note in newNotifications)
                {
                    note.UserNotified = true;
                    _notificationRepository.Save();

                    results.Add(new JsonNotification(note.Message));
                }

                //Could probably return Json(newNotifications) but that would include a bunch of potentially sensitive data
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }
    }
}
