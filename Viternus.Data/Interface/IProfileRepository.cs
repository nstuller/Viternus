using System;

namespace Viternus.Data.Interface
{
    interface IProfileRepository : IRepository<Profile>
    {
        Profile GetByEmail(string email);
        Profile New(string firstName, string lastName, string nickname, string email);
    }
}
