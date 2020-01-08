using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viternus.Data.Repository;
using Viternus.Data;

namespace Viternus.Web.Filters
{
    public class LogErrorAttribute : HandleErrorAttribute
    {

        #region IExceptionFilter Members

        public override  void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            ErrorLogRepository errorRepository = new ErrorLogRepository();
            errorRepository.SaveErrorToDB(ex, ex.Message + (ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : ""), filterContext.HttpContext.User.Identity.Name);

            base.OnException(filterContext);
        }

        #endregion
    }
}
