using System;
using System.Collections.Generic;
using Viternus.Data;

namespace Viternus.Data.Interface
{
    interface INotificationRepository : IRepository<Notification>
    {
        void AddNotificationToUser(Notification note, string userName);
        List<Notification> FindAllNewNotificationsByUser(string userName);
    }
}