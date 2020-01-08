using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viternus.Data.Interface
{
    interface IInnerCircleRepository : IRepository<InnerCircle>
    {
        List<InnerCircle> FindAllUnsentRecords();
        InnerCircle GetByEmailAndUserId(string email, Guid userId);
        void AddInnerCircleToProfile(InnerCircle newInnerCircle, Profile profile);
        void AddInnerCircleToUser(InnerCircle newInnerCircle, AppUser user);
    }
}
