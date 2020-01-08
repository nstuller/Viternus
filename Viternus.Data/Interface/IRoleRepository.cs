using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viternus.Data;

namespace Viternus.Data.Interface
{
    interface IRoleRepository : IRepository<Role>
    {
        bool IsUserInRole(string username, string role);
        Role GetByRoleName(string roleName);
        List<Role> FindByRoleName(string roleName);
        void AddUserToRole(AppUser user, Role role);
        //void AddUserToRole(AppUser user, Role role, int? externalSubId);
        void RemoveUserFromRole(AppUser user, Role role);
    }
}
