using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.DeliveryAutomation;

namespace Viternus.Web.Filters
{
    public class EmailErrorAttribute : LogErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            base.OnException(filterContext);

            System.Net.Mail.MailMessage mailItem2 = new System.Net.Mail.MailMessage();
            mailItem2.To.Add("Stullern@Yahoo.com");
            mailItem2.From = new System.Net.Mail.MailAddress("MessageDelivery@Viternus.com");
            mailItem2.Subject = "Either it finished or there was an error!";
            Exception ex = filterContext.Exception;
            mailItem2.Body = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace
                + Environment.NewLine + Environment.NewLine + ex.InnerException != null ? ex.InnerException.Message : "";
            Emailer.SendEmail(mailItem2, null, EmailEntityType.None);

            filterContext.ExceptionHandled = true;
        }
    }
}
