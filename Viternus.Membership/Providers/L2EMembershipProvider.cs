namespace Viternus.Membership.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using Viternus.Data;
    using Viternus.Data.Repository;

    public class L2EMembershipProvider : MembershipProvider
    {
        #region Private Member variables

        private const int PASSWORD_SIZE = 14;

        private bool _enablePasswordRetrieval;
        private bool _enablePasswordReset;
        private bool _requiresQuestionAndAnswer;
        private string _appName;
        private bool _requiresUniqueEmail;
        private int _maxInvalidPasswordAttempts;
        private int _passwordAttemptWindow;
        private int _minRequiredPasswordLength;
        private int _minRequiredNonalphanumericCharacters;
        private string _passwordStrengthRegularExpression = String.Empty;
        private Regex _passwordStrengthRegEx;
        private MembershipPasswordFormat _passwordFormat;

        private ProfileRepository _profRepository = new ProfileRepository();
        private AppUserRepository _userRepository = new AppUserRepository();

        #endregion

        #region Public properties

        public override bool EnablePasswordRetrieval { get { return _enablePasswordRetrieval; } }
        public override bool EnablePasswordReset { get { return _enablePasswordReset; } }
        public override bool RequiresQuestionAndAnswer { get { return _requiresQuestionAndAnswer; } }
        public override bool RequiresUniqueEmail { get { return _requiresUniqueEmail; } }
        public override MembershipPasswordFormat PasswordFormat { get { return _passwordFormat; } }
        public override int MaxInvalidPasswordAttempts { get { return _maxInvalidPasswordAttempts; } }
        public override int PasswordAttemptWindow { get { return _passwordAttemptWindow; } }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonalphanumericCharacters; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public Regex PasswordStrengthRegex
        {
            get { return _passwordStrengthRegEx; }
        }

        public override string ApplicationName
        {
            get { return _appName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                if (value.Length > StringResources.Name_Max_Size)
                    throw new ProviderException(StringResources.Provider_application_name_too_long);
                _appName = value;
            }
        }

        #endregion

        #region Private Methods

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
        {
            string salt;
            MembershipPasswordFormat passwordFormat;
            return CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out salt, out passwordFormat);
        }

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved,
                                    out string salt, out MembershipPasswordFormat passwordFormat)
        {
            string passwdFromDB;
            int status;
            int failedPasswordAttemptCount;
            int failedPasswordAnswerAttemptCount;
            bool isPasswordCorrect;
            bool isApproved;
            DateTime lastLoginDate, lastActivityDate;

            GetPasswordWithFormat(username, updateLastLoginActivityDate, out status, out passwdFromDB, out passwordFormat, out salt, out failedPasswordAttemptCount,
                                  out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);
            if (status != 0)
                return false;

            if (!isApproved && failIfNotApproved)
                return false;

            string encodedPasswd = EncodePassword(password, (MembershipPasswordFormat)passwordFormat, salt);

            isPasswordCorrect = passwdFromDB.Equals(encodedPasswd);

            //if the user required multiple attempts to get the password right
            UpdateLoginAttemptData(username, updateLastLoginActivityDate, ref status, isPasswordCorrect, ref lastLoginDate, ref lastActivityDate);

            return isPasswordCorrect;
        }

        private void UpdateLoginAttemptData(string username, bool updateLastLoginActivityDate, ref int status,
                                              bool isPasswordCorrect, ref DateTime lastLoginDate, ref DateTime lastActivityDate)
        {
            AppUser userToUpdate = _userRepository.GetByUserName(username);

            //userToUpdate.MembershipReference.EnsureLoaded(userToUpdate);

            if (userToUpdate.IsLockedOut)
                status = 99; //Account Locked Out

            if (isPasswordCorrect)
            {
                UpdateCorrectPasswordLoginAttempt(userToUpdate);
            }
            else
            {
                UpdateWrongPasswordLoginAttempt(userToUpdate);
            }

            if (updateLastLoginActivityDate)
            {
                userToUpdate.LastActivityDate = isPasswordCorrect ? DateTime.UtcNow : lastActivityDate;
                userToUpdate.LastLogOnDate = isPasswordCorrect ? DateTime.UtcNow : lastLoginDate;
            }

            _userRepository.Save();
            status = 0;
        }

        private static void UpdateCorrectPasswordLoginAttempt(AppUser userToUpdate)
        {
            userToUpdate.LastLockoutDate = new DateTime(1754, 1, 1);
            userToUpdate.FailedPasswordAttemptCount = 0;
            userToUpdate.FailedPasswordAttemptDate = new DateTime(1754, 1, 1);
            userToUpdate.FailedPasswordAnswerAttemptCount = 0;
            userToUpdate.FailedPasswordAnswerAttemptDate = new DateTime(1754, 1, 1);
        }

        private void UpdateWrongPasswordLoginAttempt(AppUser userToUpdate)
        {
            userToUpdate.FailedPasswordAttemptDate = DateTime.UtcNow;

            if (DateTime.UtcNow > userToUpdate.FailedPasswordAttemptDate.AddMinutes(PasswordAttemptWindow))
            {
                userToUpdate.FailedPasswordAnswerAttemptCount = 1;
            }
            else
            {
                userToUpdate.FailedPasswordAnswerAttemptCount += 1;
            }

            if (userToUpdate.FailedPasswordAttemptCount >= MaxInvalidPasswordAttempts)
            {
                userToUpdate.IsLockedOut = true;
                userToUpdate.LastLockoutDate = DateTime.UtcNow;
            }
        }

        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password,
                                            out MembershipPasswordFormat passwordFormat, out string passwordSalt,
                                            out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount,
                                            out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
        {
            AppUser user;
            user = GetUserWithPasswordFromDB(username, updateLastLoginActivityDate, out status);

            if (null != user)
            {
                password = user.Password;
                passwordFormat = (MembershipPasswordFormat)user.PasswordFormat;
                passwordSalt = user.PasswordSalt;
                failedPasswordAttemptCount = user.FailedPasswordAttemptCount;
                failedPasswordAnswerAttemptCount = user.FailedPasswordAnswerAttemptCount;
                isApproved = user.IsApproved;
                lastLoginDate = user.LastLogOnDate;
                lastActivityDate = user.LastActivityDate;
            }
            else
            {
                password = String.Empty;
                passwordFormat = MembershipPasswordFormat.Clear;
                passwordSalt = String.Empty;
                failedPasswordAttemptCount = 0;
                failedPasswordAnswerAttemptCount = 0;
                isApproved = false;
                lastLoginDate = DateTime.MinValue;
                lastActivityDate = DateTime.MinValue;
            }
        }

        private string GetPassword(string username, string passwordAnswer, out MembershipPasswordFormat passwordFormat, out int status)
        {
            AppUser user = GetUserWithPasswordFromDB(username, passwordAnswer, out passwordFormat, out status);

            return user.Password;
        }

        private AppUser GetUserWithPasswordFromDB(string username, bool updateLastLoginActivityDate, out int status)
        {
            AppUser user = _userRepository.GetByUserName(username);

            if (null == user)
            {
                status = 1;
                return user;
            }

            if (updateLastLoginActivityDate)
            {
                user.LastActivityDate = DateTime.UtcNow;
                _userRepository.Save();
            }

            //user.MembershipReference.EnsureLoaded(user);

            status = 0;

            if (user.IsLockedOut)
                status = 99;

            return user;
        }

        private AppUser GetUserWithPasswordFromDB(string username, string passwordAnswer, out MembershipPasswordFormat passwordFormat, out int status)
        {
            AppUser user = GetUserWithPasswordFromDB(username, false, out status);

            if (!string.IsNullOrEmpty(passwordAnswer))
            {
                if (passwordAnswer.ToUpperInvariant() != user.PasswordAnswer.ToUpperInvariant())
                {
                    user.FailedPasswordAnswerAttemptDate = DateTime.UtcNow;
                    status = 3; //Wrong Password Answer
                    if (DateTime.UtcNow > user.FailedPasswordAnswerAttemptDate.AddMinutes(PasswordAttemptWindow))
                    {
                        user.FailedPasswordAnswerAttemptCount = 1;
                    }
                    else
                    {
                        user.FailedPasswordAnswerAttemptCount += 1;
                    }

                    if (user.FailedPasswordAnswerAttemptCount >= MaxInvalidPasswordAttempts)
                    {
                        user.IsLockedOut = true;
                        user.LastLockoutDate = DateTime.UtcNow;
                    }
                }
                else
                {
                    user.FailedPasswordAnswerAttemptCount = 0;
                    user.FailedPasswordAnswerAttemptDate = new DateTime(1754, 1, 1);
                }
            }

            passwordFormat = (MembershipPasswordFormat)user.PasswordFormat;

            return user;
        }

        private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
        {
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }
            if (string.IsNullOrEmpty(passwordAnswer))
            {
                return passwordAnswer;
            }

            int status, failedPasswordAttemptCount, failedPasswordAnswerAttemptCount;
            MembershipPasswordFormat passwordFormat;
            string password, passwordSalt;
            bool isApproved;
            DateTime lastLoginDate, lastActivityDate;
            GetPasswordWithFormat(username, false, out status, out password, out passwordFormat, out passwordSalt,
                                  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);
            if (status == 0)
                return EncodePassword(passwordAnswer.ToUpperInvariant(), passwordFormat, passwordSalt);
            else
                throw new ProviderException(GetExceptionText(status));
        }

        private string GetExceptionText(int status)
        {
            string key;
            switch (status)
            {
                case 0:
                    return String.Empty;
                case 1:
                    key = StringResources.Membership_UserNotFound;
                    break;
                case 2:
                    key = StringResources.Membership_WrongPassword;
                    break;
                case 3:
                    key = StringResources.Membership_WrongAnswer;
                    break;
                case 4:
                    key = StringResources.Membership_InvalidPassword;
                    break;
                case 5:
                    key = StringResources.Membership_InvalidQuestion;
                    break;
                case 6:
                    key = StringResources.Membership_InvalidAnswer;
                    break;
                case 7:
                    key = StringResources.Membership_InvalidEmail;
                    break;
                case 99:
                    key = StringResources.Membership_AccountLockOut;
                    break;
                default:
                    key = StringResources.Provider_Error;
                    break;
            }
            return key;
        }

        private bool IsStatusDueToBadPassword(int status)
        {
            return (status >= 2 && status <= 6 || status == 99);
        }

        //new method
        private MembershipUser GetMembershipUserFromAppUser(AppUser userToUpdate)
        {
            //TODO: This line of code is causing problems when called from a loop
            //if (!userToUpdate.Profile.IsLoaded) //Watch this, cannot load a parent entry like this
            //userToUpdate.Profile.Load();

            Guid id = userToUpdate.Id;
            string email = userToUpdate.Profile.Email;
            string passwordQuestion = userToUpdate.PasswordQuestion;
            string comment = userToUpdate.Comment;
            bool isApproved = userToUpdate.IsApproved;
            DateTime dtCreate = userToUpdate.CreationDate.ToLocalTime();
            DateTime dtLastLogin = userToUpdate.LastLogOnDate.ToLocalTime();
            DateTime dtLastActivity = userToUpdate.LastActivityDate.ToLocalTime();
            DateTime dtLastPassChange = userToUpdate.LastPasswordChangedDate.ToLocalTime();
            string userName = userToUpdate.UserName;
            bool isLockedOut = userToUpdate.IsLockedOut;
            DateTime dtLastLockoutDate = userToUpdate.LastLockoutDate.ToLocalTime();

            // Return the result
            return new MembershipUser(this.Name,
                                       userName,
                                       id,
                                       email,
                                       passwordQuestion,
                                       comment,
                                       isApproved,
                                       isLockedOut,
                                       dtCreate,
                                       dtLastLogin,
                                       dtLastActivity,
                                       dtLastPassChange,
                                       dtLastLockoutDate);
        }

        private void InitPasswordStrength(NameValueCollection config)
        {
            _passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (_passwordStrengthRegularExpression != null)
            {
                _passwordStrengthRegularExpression = _passwordStrengthRegularExpression.Trim();
                if (_passwordStrengthRegularExpression.Length != 0)
                {
                    try
                    {
                        //Call the constructor of Regex to make sure that the configured regular expression is valid
                        _passwordStrengthRegEx = new Regex(_passwordStrengthRegularExpression);
                    }
                    catch (ArgumentException e)
                    {
                        throw new ProviderException(e.Message, e);
                    }
                }
            }
            else
            {
                _passwordStrengthRegularExpression = string.Empty;
            }
            if (_minRequiredNonalphanumericCharacters > _minRequiredPasswordLength)
                throw new ProviderException(StringResources.MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength);
        }

        private void SetInitVars(NameValueCollection config)
        {
            _enablePasswordRetrieval = SecurityUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
            _enablePasswordReset = SecurityUtility.GetBooleanValue(config, "enablePasswordReset", true);
            _requiresQuestionAndAnswer = SecurityUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            _requiresUniqueEmail = SecurityUtility.GetBooleanValue(config, "requiresUniqueEmail", false);
            _maxInvalidPasswordAttempts = SecurityUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            _passwordAttemptWindow = SecurityUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            _minRequiredPasswordLength = SecurityUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, StringResources.Password_Max_Size);
            _minRequiredNonalphanumericCharacters = SecurityUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, StringResources.Password_Max_Size);
        }

        private bool ValidatePasswordStrength(string password, ref MembershipCreateStatus status)
        {
            if (password.Length < MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return false;
            }

            int count = 0;

            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i))
                {
                    count++;
                }
            }

            if (count < MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return false;
            }

            if (PasswordStrengthRegularExpression.Length > 0)
            {
                if (!Regex.IsMatch(password, PasswordStrengthRegularExpression))
                {
                    status = MembershipCreateStatus.InvalidPassword;
                    return false;
                }
            }

            return true;
        }

        private bool ValidateUserUniqueness(MembershipUser user)
        {
            return (MembershipCreateStatus.Success == ValidateUserUniqueness(user.UserName, user.Email, user.ProviderUserKey));
        }

        private MembershipCreateStatus ValidateUserUniqueness(string facebookId)
        {
            Profile prof = _profRepository.GetByFacebookId(facebookId);
            if (null != prof)
            {
                return MembershipCreateStatus.DuplicateProviderUserKey;
            }

            return MembershipCreateStatus.Success;
        }

        /// <summary>
        /// Checks to see if the user already exists in the database before update or create.  This method checks both my username and email address.  The providerUserKey is used
        /// on updates to determine if the user that exists is the same user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="providerUserKey">In the case of a create, this will be null</param>
        /// <returns></returns>
        private MembershipCreateStatus ValidateUserUniqueness(string username, string email, object providerUserKey)
        {
            if (RequiresUniqueEmail)
            {
                //Check For Duplicate Email Addresses
                List<AppUser> usersWithThisEmail = _userRepository.FindByEmail(email);
                if (1 < usersWithThisEmail.Count || (1 == usersWithThisEmail.Count && (null == providerUserKey || usersWithThisEmail[0].Id.ToString() != providerUserKey.ToString())))
                    return MembershipCreateStatus.DuplicateEmail;

                //Check For Duplicate User Names
                List<AppUser> usersWithThisUserName = _userRepository.FindByUserName(username);
                if (1 < usersWithThisUserName.Count || (1 == usersWithThisUserName.Count && (null == providerUserKey || usersWithThisUserName[0].Id.ToString() != providerUserKey.ToString())))
                    return MembershipCreateStatus.DuplicateUserName;
            }

            return MembershipCreateStatus.Success;
        }

        private bool VerifyCreateUserParameters(ref string username, ref string email, ref string passwordQuestion,
                                                    string passwordAnswer, ref MembershipCreateStatus status,
                                                    string salt, object providerUserKey, ref string encodedPasswordAnswer)
        {
            if (null != providerUserKey)
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(passwordAnswer))
            {
                passwordAnswer = passwordAnswer.Trim();
                if (passwordAnswer.Length > StringResources.Password_Max_Size)
                {
                    status = MembershipCreateStatus.InvalidAnswer;
                    return false;
                }
                encodedPasswordAnswer = EncodePassword(passwordAnswer.ToUpperInvariant(), _passwordFormat, salt);
            }
            else
            {
                encodedPasswordAnswer = passwordAnswer;
            }

            if (!SecurityUtility.ValidateParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer,
                RequiresQuestionAndAnswer, false, StringResources.Password_Max_Size))
            {
                status = MembershipCreateStatus.InvalidAnswer;
                return false;
            }

            if (!SecurityUtility.ValidateParameter(ref username, true, true, true, StringResources.Name_Max_Size))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return false;
            }

            if (!SecurityUtility.ValidateParameter(ref email, RequiresUniqueEmail, RequiresUniqueEmail, false, StringResources.Name_Max_Size))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return false;
            }

            if (!SecurityUtility.ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Name_Max_Size))
            {
                status = MembershipCreateStatus.InvalidQuestion;
                return false;
            }

            return true;
        }

        private AppUser SaveNewUserData(string username, string email, string passwordQuestion, bool isApproved,
                                                    object providerUserKey, string salt, string pass, string encodedPasswordAnswer)
        {
            AppUser appUserDetail;
            if (null == providerUserKey)
                appUserDetail = _userRepository.New();
            else
                appUserDetail = _userRepository.New((Guid)providerUserKey);

            appUserDetail.UserName = username;
            appUserDetail.LastActivityDate = DateTime.UtcNow;
            appUserDetail.Password = pass;
            appUserDetail.PasswordSalt = salt;
            appUserDetail.PasswordFormat = (int)PasswordFormat;
            appUserDetail.PasswordQuestion = passwordQuestion;
            appUserDetail.PasswordAnswer = encodedPasswordAnswer;
            appUserDetail.IsApproved = isApproved;
            appUserDetail.CreationDate = DateTime.UtcNow;
            appUserDetail.IsLockedOut = false;
            appUserDetail.LastLogOnDate = new DateTime(1754, 1, 1);
            appUserDetail.LastPasswordChangedDate = DateTime.UtcNow;
            appUserDetail.LastLockoutDate = new DateTime(1754, 1, 1);
            appUserDetail.FailedPasswordAnswerAttemptDate = new DateTime(1754, 1, 1);
            appUserDetail.FailedPasswordAttemptDate = new DateTime(1754, 1, 1);
            appUserDetail.FailedPasswordAnswerAttemptCount = 0;
            appUserDetail.FailedPasswordAttemptCount = 0;

            Profile profDetail = _profRepository.GetByEmail(email);
            if (null == profDetail)
                profDetail = _profRepository.New();

            profDetail.Email = email;
            _profRepository.Save(profDetail);

            _profRepository.AddUserToProfile(appUserDetail, profDetail);
            _userRepository.Save(appUserDetail);

            return appUserDetail;
        }

        private void CheckPasswordEventArgs(string username, string newPassword)
        {
            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);

            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                else
                {
                    throw new ArgumentException(StringResources.Membership_Custom_Password_Validation_Failure, "newPassword");
                }
            }
        }

        private string CheckNewPassword(string newPassword, string salt, MembershipPasswordFormat passwordFormat)
        {
            MembershipCreateStatus placeHolder = new MembershipCreateStatus();
            return CheckNewPassword(newPassword, salt, passwordFormat, ref placeHolder);
        }

        private string CheckNewPassword(string newPassword, string salt, MembershipPasswordFormat passwordFormat, ref MembershipCreateStatus status)
        {
            if (!SecurityUtility.ValidateParameter(ref newPassword, true, true, false, StringResources.Password_Max_Size))
            {
                status = MembershipCreateStatus.InvalidPassword;
            }
            else
            {
                //If invalid, will set the status to "Invalid_Password"
                ValidatePasswordStrength(newPassword, ref status);
            }

            string pass = EncodePassword(newPassword, passwordFormat, salt);
            return pass;
        }

        private static void CheckEventArgs(ValidatePasswordEventArgs e)
        {
            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                else
                {
                    throw new ProviderException(StringResources.Membership_Custom_Password_Validation_Failure);
                }
            }
        }

        private string GetEncodedPassword(string answer, string salt, MembershipPasswordFormat passwordFormat)
        {
            if (answer != null)
                answer = answer.Trim();

            string encodedPasswordAnswer;
            if (!string.IsNullOrEmpty(answer))
                encodedPasswordAnswer = EncodePassword(answer.ToUpperInvariant(), passwordFormat, salt);
            else
                encodedPasswordAnswer = answer;
            SecurityUtility.CheckParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Password_Max_Size, "passwordAnswer");
            return encodedPasswordAnswer;
        }

        private void CheckResetPasswordStatus(int status)
        {
            if (status != 0)
            {
                if (IsStatusDueToBadPassword(status))
                {
                    throw new MembershipPasswordException(GetExceptionText(status));
                }
                else
                {
                    throw new ProviderException(GetExceptionText(status));
                }
            }
        }

        private int UpdateResetPassword(string username, string salt, MembershipPasswordFormat passwordFormat, int status, string encodedPasswordAnswer, string newPassword)
        {
            AppUser userToUpdate = _userRepository.GetByUserName(username);

            //userToUpdate.MembershipReference.EnsureLoaded(userToUpdate);

            if (userToUpdate.IsLockedOut)
                status = 99; //Account Locked Out

            if (RequiresQuestionAndAnswer && encodedPasswordAnswer.ToUpperInvariant() != userToUpdate.PasswordAnswer.ToUpperInvariant())
            {
                userToUpdate.FailedPasswordAnswerAttemptDate = DateTime.UtcNow;
                status = 3; //Wrong Password Answer
                if (DateTime.UtcNow > userToUpdate.FailedPasswordAnswerAttemptDate.AddMinutes(PasswordAttemptWindow))
                {
                    userToUpdate.FailedPasswordAnswerAttemptCount = 1;
                }
                else
                {
                    userToUpdate.FailedPasswordAnswerAttemptCount += 1;
                }

                if (userToUpdate.FailedPasswordAnswerAttemptCount >= MaxInvalidPasswordAttempts)
                {
                    userToUpdate.IsLockedOut = true;
                    userToUpdate.LastLockoutDate = DateTime.UtcNow;
                }
            }
            else
            {
                userToUpdate.Password = EncodePassword(newPassword, passwordFormat, salt);
                userToUpdate.PasswordSalt = salt;
                userToUpdate.PasswordFormat = (int)passwordFormat;
                userToUpdate.LastPasswordChangedDate = DateTime.UtcNow;
                userToUpdate.FailedPasswordAnswerAttemptCount = 0;
                userToUpdate.FailedPasswordAnswerAttemptDate = new DateTime(1754, 1, 1);
            }

            _userRepository.Save();

            return status;
        }

        #endregion

        #region Internal Methods

        internal string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        internal string EncodePassword(string pass, MembershipPasswordFormat passwordFormat, string salt)
        {
            if (0 == passwordFormat) // MembershipPasswordFormat.Clear
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            if (MembershipPasswordFormat.Hashed == passwordFormat)// MembershipPasswordFormat.Hashed
            {
                HashAlgorithm s = HashAlgorithm.Create(Membership.HashAlgorithmType);
                bRet = s.ComputeHash(bAll);
            }
            else
            {
                bRet = EncryptPassword(bAll);
            }

            return Convert.ToBase64String(bRet);
        }

        internal string UnEncodePassword(string pass, MembershipPasswordFormat passwordFormat)
        {
            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Clear: // MembershipPasswordFormat.Clear:
                    return pass;
                case MembershipPasswordFormat.Hashed: // MembershipPasswordFormat.Hashed:
                    throw new ProviderException(StringResources.Provider_can_not_decode_hashed_password);
                default:
                    byte[] bIn = Convert.FromBase64String(pass);
                    byte[] bRet = DecryptPassword(bIn);
                    if (bRet == null)
                        return null;
                    return Encoding.Unicode.GetString(bRet, 16, bRet.Length - 16);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the Entity Framework membership provider with the property values specified in the application's configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            NameValueCollection values = new NameValueCollection(config);

            if (String.IsNullOrEmpty(name))
                name = "L2EMembershipProvider";

            if (string.IsNullOrEmpty(values["description"]))
            {
                values.Remove("description");
                values.Add("description", StringResources.MembershipL2EProvider_description);
            }
            base.Initialize(name, values);

            SetInitVars(values);

            InitPasswordStrength(values);

            _appName = values["applicationName"];
            if (string.IsNullOrEmpty(_appName))
                ApplicationName = SecurityUtility.DefaultAppName;

            if (_appName.Length > StringResources.Name_Max_Size)
            {
                throw new ProviderException(StringResources.Provider_application_name_too_long);
            }

            string strTemp = values["passwordFormat"] ?? MembershipPasswordFormat.Hashed.ToString();

            try { _passwordFormat = (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat), strTemp); }
            catch { throw new ProviderException(StringResources.Provider_bad_password_format); }

            if (MembershipPasswordFormat.Hashed == PasswordFormat && EnablePasswordRetrieval)
                throw new ProviderException(StringResources.Provider_can_not_retrieve_hashed_password);

            //if (_passwordFormat == MembershipPasswordFormat.Encrypted && MachineKeySection.IsDecryptionKeyAutogenerated)
            //    throw new ProviderException(String.Format(StringResources.Can_not_use_encrypted_passwords_with_autogen_keys));

            values.Remove("connectionStringName");
            values.Remove("enablePasswordRetrieval");
            values.Remove("enablePasswordReset");
            values.Remove("requiresQuestionAndAnswer");
            values.Remove("applicationName");
            values.Remove("requiresUniqueEmail");
            values.Remove("maxInvalidPasswordAttempts");
            values.Remove("passwordAttemptWindow");
            values.Remove("commandTimeout");
            values.Remove("passwordFormat");
            values.Remove("name");
            values.Remove("minRequiredPasswordLength");
            values.Remove("minRequiredNonalphanumericCharacters");
            values.Remove("passwordStrengthRegularExpression");
            if (values.Count > 0)
            {
                string attribUnrecognized = values.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Provider_unrecognized_attribute, attribUnrecognized));
            }
        }

        public MembershipUser CreateUser(string username, string password, string email, 
                                                   bool isApproved, object providerUserKey, string facebookId, out MembershipCreateStatus status, bool agreedToTOS)
        {
            status = ValidateUserUniqueness(facebookId);

            MembershipUser newUser = null;
            if (MembershipCreateStatus.Success == status)
                newUser = CheckParamsAndCreateUser(username, password, email, null, null, isApproved, providerUserKey, ref status, null, null);

            if (null != newUser && MembershipCreateStatus.Success == status)
            {
                //Set the facebook Id
                AppUser userToUpdate = _userRepository.GetByUserName(newUser.UserName);
                userToUpdate.Profile.FacebookId = facebookId;
                userToUpdate.AgreedToTOS = agreedToTOS;
                _userRepository.Save(userToUpdate);
            }

            return newUser;
        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                   bool isApproved, object providerUserKey, out MembershipCreateStatus status, bool agreedToTOS)
        {
            MembershipUser newUser = CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);

            if (null != newUser)
            {
                //Set the facebook Id
                AppUser userToUpdate = _userRepository.GetByUserName(newUser.UserName);
                userToUpdate.AgreedToTOS = agreedToTOS;
                _userRepository.Save(userToUpdate);
            }

            return newUser;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                   bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.Success;

            string salt = GenerateSalt();
            string pass = CheckNewPassword(password, salt, _passwordFormat, ref status);

            return CheckParamsAndCreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, ref status, salt, pass);
        }

        private MembershipUser CheckParamsAndCreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, ref MembershipCreateStatus status, string salt, string pass)
        {
            if (MembershipCreateStatus.Success != status)
            {
                return null;
            }

            string encodedPasswordAnswer = "";

            if (!VerifyCreateUserParameters(ref username, ref email, ref passwordQuestion, passwordAnswer, ref status, salt, providerUserKey, ref encodedPasswordAnswer))
                return null;

            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(e);
            if (e.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            status = ValidateUserUniqueness(username, email, null);

            if (MembershipCreateStatus.Success == status)
            {
                AppUser appUserDetail = SaveNewUserData(username, email, passwordQuestion, isApproved, providerUserKey, salt, pass, encodedPasswordAnswer);
                return GetMembershipUserFromAppUser(appUserDetail);
            }
            else
            {
                return null;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            SecurityUtility.CheckParameter(ref username, true, true, true, StringResources.Name_Max_Size, "username");
            SecurityUtility.CheckParameter(ref password, true, true, false, StringResources.Password_Max_Size, "password");

            string salt;
            MembershipPasswordFormat passwordFormat;
            if (!CheckPassword(username, password, false, false, out salt, out passwordFormat))
                return false;

            SecurityUtility.CheckParameter(ref newPasswordQuestion, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Name_Max_Size, "newPasswordQuestion");

            if (newPasswordAnswer != null)
                newPasswordAnswer = newPasswordAnswer.Trim();

            SecurityUtility.CheckParameter(ref newPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Password_Max_Size, "newPasswordAnswer");

            string encodedPasswordAnswer;
            if (!string.IsNullOrEmpty(newPasswordAnswer))
            {
                encodedPasswordAnswer = EncodePassword(newPasswordAnswer.ToUpperInvariant(), passwordFormat, salt);
            }
            else
            {
                encodedPasswordAnswer = newPasswordAnswer;
            }
            SecurityUtility.CheckParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Password_Max_Size, "newPasswordAnswer");

            AppUser userToUpdate = _userRepository.GetByUserName(username);
            //userToUpdate.MembershipReference.EnsureLoaded(userToUpdate);
            userToUpdate.PasswordQuestion = newPasswordQuestion;
            userToUpdate.PasswordAnswer = encodedPasswordAnswer;
            _userRepository.Save();

            return (true);
        }

        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new NotSupportedException(StringResources.Membership_PasswordRetrieval_not_supported);
            }

            SecurityUtility.CheckParameter(ref username, true, true, true, StringResources.Name_Max_Size, "username");

            string encodedPasswordAnswer = GetEncodedPasswordAnswer(username, answer);
            SecurityUtility.CheckParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, StringResources.Password_Max_Size, "passwordAnswer");

            string errText;
            MembershipPasswordFormat passwordFormat = MembershipPasswordFormat.Clear;
            int status = 0;

            string pass = GetPassword(username, encodedPasswordAnswer, out passwordFormat, out status);

            if (pass == null)
            {
                errText = GetExceptionText(status);
                if (IsStatusDueToBadPassword(status))
                {
                    throw new MembershipPasswordException(errText);
                }
                else
                {
                    throw new ProviderException(errText);
                }
            }

            return UnEncodePassword(pass, passwordFormat);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            SecurityUtility.CheckParameter(ref username, true, true, true, StringResources.Name_Max_Size, "username");
            SecurityUtility.CheckParameter(ref oldPassword, true, true, false, StringResources.Password_Max_Size, "oldPassword");
            SecurityUtility.CheckParameter(ref newPassword, true, true, false, StringResources.Password_Max_Size, "newPassword");

            string salt = null;
            MembershipPasswordFormat passwordFormat;

            if (!CheckPassword(username, oldPassword, false, false, out salt, out passwordFormat))
                return false;

            string pass = CheckNewPassword(newPassword, salt, passwordFormat);

            CheckPasswordEventArgs(username, newPassword);

            AppUser userToUpdate = _userRepository.GetByUserName(username);

            //userToUpdate.MembershipReference.EnsureLoaded(userToUpdate);

            userToUpdate.Password = pass;
            userToUpdate.PasswordSalt = salt;
            userToUpdate.PasswordFormat = (int)passwordFormat;
            userToUpdate.LastPasswordChangedDate = DateTime.UtcNow;

            _userRepository.Save();

            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException(StringResources.Not_configured_to_support_password_resets);
            }

            SecurityUtility.CheckParameter(ref username, true, true, true, StringResources.Name_Max_Size, "username");

            string salt;
            MembershipPasswordFormat passwordFormat;
            string passwdFromDB;
            int status;
            int failedPasswordAttemptCount;
            int failedPasswordAnswerAttemptCount;
            bool isApproved;
            DateTime lastLoginDate, lastActivityDate;

            GetPasswordWithFormat(username, false, out status, out passwdFromDB, out passwordFormat, out salt, out failedPasswordAttemptCount,
                                  out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);

            //check the password status and throw if bad
            CheckResetPasswordStatus(status);

            string encodedPasswordAnswer = GetEncodedPassword(answer, salt, passwordFormat);
            string newPassword = GeneratePassword();

            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);
            CheckEventArgs(e);

            status = UpdateResetPassword(username, salt, passwordFormat, status, encodedPasswordAnswer, newPassword);

            CheckResetPasswordStatus(status);
            return newPassword;
        }

        public override void UpdateUser(MembershipUser user)
        {
            string temp = user.UserName;
            SecurityUtility.CheckParameter(ref temp, true, true, true, StringResources.Name_Max_Size, "UserName");
            temp = user.Email;
            SecurityUtility.CheckParameter(ref temp, RequiresUniqueEmail, RequiresUniqueEmail, false, StringResources.Name_Max_Size, "Email");
            user.Email = temp;

            if (!ValidateUserUniqueness(user))
            {
                throw new ProviderException(GetExceptionText(1));
            }

            AppUser userToUpdate = _userRepository.GetByUserName(user.UserName);
            userToUpdate.LastActivityDate = user.LastActivityDate.ToUniversalTime();
            userToUpdate.UserName = user.UserName;
            userToUpdate.Profile.Email = user.Email;
            userToUpdate.Comment = user.Comment;
            userToUpdate.IsApproved = user.IsApproved;
            userToUpdate.LastLogOnDate = user.LastLoginDate.ToUniversalTime();

            _userRepository.Save(userToUpdate);
        }

        /// <summary>
        /// Validates username and password to determine if it represents a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if (SecurityUtility.ValidateParameter(ref username, true, true, true, StringResources.Name_Max_Size) &&
                    SecurityUtility.ValidateParameter(ref password, true, true, false, StringResources.Password_Max_Size) &&
                    CheckPassword(username, password, true, true))
            {
                // Comment out perf counters in sample: PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_SUCCESS);
                // Comment out events in sample: WebBaseEvent.RaiseSystemEvent(null, WebEventCodes.AuditMembershipAuthenticationSuccess, username);
                return true;
            }
            else
            {
                // Comment out perf counters in sample: PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_FAIL);
                // Comment out events in sample: WebBaseEvent.RaiseSystemEvent(null, WebEventCodes.AuditMembershipAuthenticationFailure, username);
                return false;
            }
        }

        /// <summary>
        /// Clears the user's locked-out status so that the membership user can be validated.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Whether or not the save was successful</returns>
        public override bool UnlockUser(string userName)
        {
            SecurityUtility.CheckParameter(ref userName, true, true, true, StringResources.Name_Max_Size, "username");

            AppUser userToUpdate = _userRepository.GetByUserName(userName);

            userToUpdate.LastLockoutDate = new DateTime(1754, 1, 1);
            userToUpdate.FailedPasswordAttemptCount = 0;
            userToUpdate.FailedPasswordAttemptDate = new DateTime(1754, 1, 1);
            userToUpdate.IsLockedOut = false;
            userToUpdate.FailedPasswordAnswerAttemptCount = 0;
            userToUpdate.FailedPasswordAnswerAttemptDate = new DateTime(1754, 1, 1);

            _userRepository.Save();

            return true;
        }

        /// <summary>
        /// Overloaded. Gets the information for a membership user from the data source.
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null)
            {
                throw new ArgumentNullException("providerUserKey");
            }

            if (!(providerUserKey is Guid))
            {
                throw new ArgumentException(StringResources.Membership_InvalidProviderUserKey, "providerUserKey");
            }

            AppUser userToUpdate = _userRepository.GetById((Guid)providerUserKey);

            if (userIsOnline)
            {
                userToUpdate.LastActivityDate = DateTime.UtcNow;
                _userRepository.Save();
            }

            return GetMembershipUserFromAppUser(userToUpdate);
        }

        /// <summary>
        /// Overloaded. Gets the information for a membership user from the data source.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            SecurityUtility.CheckParameter(ref username, true, false, true, StringResources.Name_Max_Size, "username");

            AppUser userToUpdate = _userRepository.GetByUserName(username);

            if (userIsOnline)
            {
                userToUpdate.LastActivityDate = DateTime.UtcNow;
                _userRepository.Save();
            }

            return GetMembershipUserFromAppUser(userToUpdate);
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            SecurityUtility.CheckParameter(ref email, false, false, false, StringResources.Name_Max_Size, "email");

            AppUser user = _userRepository.GetByEmail(email);

            return user.UserName;
        }

        /// <summary>
        /// Removes a user's membership information from the Entity Framework membership database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            SecurityUtility.CheckParameter(ref username, true, true, true, StringResources.Name_Max_Size, "username");

            //SqlCommand cmd = new SqlCommand("dbo.aspnet_Users_DeleteUser", holder.Connection);

            if (deleteAllRelatedData)
            {
                AppUser userToDelete = _userRepository.GetByUserName(username);

                //TODO: This will likely need to be more robust (delete records from all related tables)
                //Call Ensure Loaded on all related tables... then the Entity Framework takes care of deleting them all
                //Make sure it doesn't delete a role just because a user was deleted, it should delete the cross-join table though
                if (!userToDelete.Role.IsLoaded)
                    userToDelete.Role.Load();

                _userRepository.Delete(userToDelete);
            }
            else
            {
                //TraceUtility.Log("{0} parameter should always be set to true.  The parameter is only preserved so that the Membership Provider is not broken.", TraceLevel.Warning, "deleteAllRelatedData");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a collection of all the users in the Entity Framework membership database.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //TODO: Implement paging on GetAllUsers
            if (pageIndex < 0)
                throw new ArgumentException(StringResources.PageIndex_bad, "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException(StringResources.PageSize_bad, "pageSize");

            long upperBound = (long)pageIndex * pageSize + pageSize - 1;
            if (upperBound > Int32.MaxValue)
                throw new ArgumentException(StringResources.PageIndex_PageSize_bad, "pageSize");

            MembershipUserCollection users = new MembershipUserCollection();

            List<AppUser> usersFromDB = _userRepository.GetAll();
            totalRecords = 0;

            foreach (var userDetail in usersFromDB)
            {
                users.Add(GetMembershipUserFromAppUser(userDetail));
                totalRecords++; //Probably not a good way to assign this.  Can just do a count or something.
            }

            return users;
        }

        /// <summary>
        /// Returns the number of users currently accessing the application.
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            //This method is stubbed out even though it was implemented because we don't know how to test it.
            throw new NotImplementedException("Below code can be uncommented if this method is needed.");
            //return _userRepository.GetNumberOfUsersOnLine(Membership.UserIsOnlineTimeWindow);
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            SecurityUtility.CheckParameter(ref usernameToMatch, true, true, false, StringResources.Name_Max_Size, "usernameToMatch");

            if (pageIndex < 0)
                throw new ArgumentException(StringResources.PageIndex_bad, "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException(StringResources.PageSize_bad, "pageSize");

            long upperBound = (long)pageIndex * pageSize + pageSize - 1;
            if (upperBound > Int32.MaxValue)
                throw new ArgumentException(StringResources.PageIndex_PageSize_bad, "pageSize");

            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            List<AppUser> usersFromDB = _userRepository.FindByUserName(usernameToMatch);

            foreach (var userDetail in usersFromDB)
            {
                users.Add(GetMembershipUserFromAppUser(userDetail));
                totalRecords++; //Probably not a good way to assign this.  Can just do a count or something.
            }

            return users;
        }

        /// <summary>
        /// Returns a collection of membership users for which the e-mail address field contains the specified e-mail address.
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            SecurityUtility.CheckParameter(ref emailToMatch, true, true, false, StringResources.Name_Max_Size, "emailToMatch");

            if (pageIndex < 0)
                throw new ArgumentException(StringResources.PageIndex_bad, "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException(StringResources.PageSize_bad, "pageSize");

            long upperBound = (long)pageIndex * pageSize + pageSize - 1;
            if (upperBound > Int32.MaxValue)
                throw new ArgumentException(StringResources.PageIndex_PageSize_bad, "pageSize");

            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            List<AppUser> usersFromDB = _userRepository.FindByEmail(emailToMatch);

            foreach (var userDetail in usersFromDB)
            {
                users.Add(GetMembershipUserFromAppUser(userDetail));
                totalRecords++; //Probably not a good way to assign this.  Can just do a count or something.
            }

            return users;
        }

        /// <summary>
        /// Generates a random password that is at least 14 characters long.
        /// </summary>
        /// <returns></returns>
        public virtual string GeneratePassword()
        {
            return Membership.GeneratePassword(MinRequiredPasswordLength < PASSWORD_SIZE ? PASSWORD_SIZE : MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);
        }

        #endregion

    }
}
