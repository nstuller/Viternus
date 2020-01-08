using System;
using System.Collections.Generic;
using Viternus.Data;

namespace Viternus.Data.Interface
{
    interface IErrorLogRepository : IRepository<ErrorLog>
    {
        void AddErrorLogToUser(ErrorLog err, string userName);
    }
}