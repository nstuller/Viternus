using System;
using System.Collections.Generic;
using Viternus.Data;

namespace Viternus.Data.Interface
{
    interface IAppUserRepository : IRepository<AppUser>
    {
        List<AppUser> FindUsersWithEventBasedMessages();
    }
}
