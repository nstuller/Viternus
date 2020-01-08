using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;

namespace Viternus.Data
{
    public partial class Profile : IEntity
    {
        public string ProposedUserName
        {
            get
            {
                string firstInit = String.IsNullOrEmpty(FirstName) ? null : FirstName.Substring(0, 1);
                return String.Format("{0}{1}", firstInit, LastName);
            }
        }
    }
}