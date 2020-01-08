using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        protected override ObjectQuery<Notification> EntityQuery
        {
            get { return DataConnector.Context.Notification; }
        }

        public void AddNotificationToUser(Notification note, string userName)
        {
            AppUserRepository userRepository = new AppUserRepository();
            userRepository.GetByUserName(userName).Notification.Add(note);
        }

        /// <summary>
        /// Returns all Videos that have recently been uploaded and the user has not been notified yet
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<Notification> FindAllNewNotificationsByUser(string userName)
        {
            var result = from n in DataConnector.Context.Notification.Include("AppUser")
                         where n.AppUser.UserName == userName
                            && !n.UserNotified
                         select n;

            return result.ToList();
        }
    }
}
