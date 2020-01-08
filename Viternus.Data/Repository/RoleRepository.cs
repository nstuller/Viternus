using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;
using System.Collections.Generic;

namespace Viternus.Data.Repository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        protected override ObjectQuery<Role> EntityQuery
        {
            get { return DataConnector.Context.Role; }
        }

        public bool IsUserInRole(string username, string role)
        {
            var results = from a in DataConnector.Context.AppUser
                          where a.UserName.ToLower() == username.ToLower() && a.Role.Any(c => c.RoleName.ToLower() == role.ToLower())
                          select a;

            return (0 < results.Count());
        }

        public Role GetByRoleName(string roleName)
        {
            var results = FindByRoleName(roleName);

            return results.FirstOrDefault();
        }

        public List<Role> FindByRoleName(string roleName)
        {
            var results = from r in EntityQuery.Include("AppUser")
                          where r.RoleName.ToLower().Contains(roleName.ToLower())
                          select r;

            return results.ToList();
        }

        public void AddUserToRole(AppUser user, Role role)
        {
            if (!IsUserInRole(user.UserName, role.RoleName))
            {
                role.AppUser.Add(user);
            }
        }

        public void RemoveUserFromRole(AppUser user, Role role)
        {
            if (IsUserInRole(user.UserName, role.RoleName))
            {
                role.AppUser.Remove(user);
            }
        }
    }
}
