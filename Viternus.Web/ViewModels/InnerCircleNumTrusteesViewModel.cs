using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.Data;

namespace Viternus.Web.ViewModels
{
    public class InnerCircleNumTrusteesViewModel
    {
        public AppUser User { get; private set; }
        public string NumberOfTrustees
        {
            get
            {
                if (null == User.Profile.NumberOfTrustees)
                    return String.Empty;
                else
                    return User.Profile.NumberOfTrustees.ToString();
            }
        }

        public InnerCircleNumTrusteesViewModel(AppUser userToAddTo)
        {
            User = userToAddTo;
        }
    }
}
