using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class ErrorLogRepository : RepositoryBase<ErrorLog>, IErrorLogRepository
    {
        protected override ObjectQuery<ErrorLog> EntityQuery
        {
            get { return DataConnector.Context.ErrorLog; }
        }

        public void AddErrorLogToUser(ErrorLog err, string userName)
        {
            AppUserRepository userRepository = new AppUserRepository();
            userRepository.GetByUserName(userName).ErrorLog.Add(err);
        }

        public void SaveErrorToDB(Exception ex, string message, string user)
        {
            ErrorLog err = New();
            err.Message = message;
            err.StackTrace = (ex != null) ? ex.StackTrace : String.Empty;
            err.CreationDate = DateTime.Now;

            string userName = user;
            if (!String.IsNullOrEmpty(userName))
                AddErrorLogToUser(err, userName);

            Save(err);
        }
    }
}
