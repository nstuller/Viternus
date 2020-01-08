using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viternus.Data.Interface;
using Viternus.Data.Repository;

namespace Viternus.Data
{
    public partial class InnerCircle : IEntity
    {
        public bool AnyDeceasedRecords
        {
            get
            {
                return (0 < this.InnerCircleAction.Where(ica => ica.EventType.Description == "Death").Count());
            }
        }

        public bool AnyIncapacitatedRecords
        {
            get
            {
                return (0 < this.InnerCircleAction.Where(ica => ica.EventType.Description == "Incapacitation").Count());
            }
        }

        public int MajorityOfTrustees
        {
            get
            {
                short numTrustees = this.AppUser.Profile.NumberOfTrustees.Value;
                int majority = numTrustees / 2 + 1;
                return (majority);
            }
        }

        public bool MajorityDeceased
        {
            get
            {
                int numRecords = this.InnerCircleAction.Where(ica => ica.EventType.Description == "Death").Count();
                return (numRecords >= MajorityOfTrustees);
            }
        }

        public bool MajorityIncapacitated
        {
            get
            {
                int numRecords = this.InnerCircleAction.Where(ica => ica.EventType.Description == "Incapacitation").Count();
                return (numRecords >= MajorityOfTrustees);
            }
        }

        public bool ExistingDeathActionFromLoggedInUser
        {
            get
            {
                InnerCircleActionRepository icaRep = new InnerCircleActionRepository();
                //HACK: instead of getting the user name from the HTTPContext, which we don't have access to, we are using the profile's of the Inner Circle
                //This works because the logged in user should only see Inner Circle records that match the profile to the logged in user name
                return null != icaRep.GetByInnerCircleAndUserNameAndEventTypeDesc(this, this.Profile.AppUser.FirstOrDefault().UserName, "Death");
            }
        }

        public bool ExistingIncapacitationActionFromLoggedInUser
        {
            get
            {
                InnerCircleActionRepository icaRep = new InnerCircleActionRepository();
                //HACK: instead of getting the user name from the HTTPContext, which we don't have access to, we are using the profile's of the Inner Circle
                //This works because the logged in user should only see Inner Circle records that match the profile to the logged in user name
                return null != icaRep.GetByInnerCircleAndUserNameAndEventTypeDesc(this, this.Profile.AppUser.FirstOrDefault().UserName, "Incapacitation");
            }
        }

        public string StatusMessage
        {
            get
            {
                if (MajorityDeceased)
                    return "This user is deceased";
                else if (ExistingDeathActionFromLoggedInUser)
                    return "You have indiciated that this user is deceased";
                else if (AnyDeceasedRecords)
                    return "Someone else has indicated that this user is deceased";
                else if (MajorityIncapacitated)
                    return "This user has been incapacitated";
                else if (ExistingIncapacitationActionFromLoggedInUser)
                    return "You have indicated that this user has been incapacitated";
                else if (AnyIncapacitatedRecords)
                    return "Someone else has indicated that this user has been incapacitated";
                else
                    return String.Empty;
            }
        }

    }
}
