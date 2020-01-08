using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.Data;
using Viternus.Data.Repository;
using System.Web.Mvc;

namespace Viternus.Web.ViewModels
{
    public class InnerCircleMemberViewModel
    {
        public AppUser User { get; private set; }
        public SelectList EventTypes { get; private set; }
        public DateTime DefaultDate { get { return DateTime.Now; } }

        public InnerCircleMemberViewModel(AppUser loggedInUser)
        {
            User = loggedInUser;

            EventTypeRepository etRep = new EventTypeRepository();
            EventTypes = new SelectList(etRep.GetAll(), "Id", "Description");
        }
    }
}
