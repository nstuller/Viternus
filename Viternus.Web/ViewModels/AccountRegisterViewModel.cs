using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viternus.Web.ViewModels
{
    public class AccountRegisterViewModel
    {
        public int PasswordLength { get; private set; }
        public int MinRequiredNonAlphanumericCharacters { get; private set; }
        public string UserName { get; private set; }
        public string EmailAddress { get; private set; }

        public AccountRegisterViewModel(int pwLen, int minSpecialChars, string userName, string email)
        {
            PasswordLength = pwLen;
            MinRequiredNonAlphanumericCharacters = minSpecialChars;
            UserName = userName;
            EmailAddress = email;
        }
    }
}
