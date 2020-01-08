

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Web.Security;
using Viternus.Data;
using Viternus.Data.Repository;

namespace Viternus.Membership.Providers
{
    /// <summary>
    /// Custom Linq to Entities Role provider (to fit into the ASP.NET Role Provider model)
    /// </summary>
    public class L2ERoleProvider : RoleProvider
    {
        #region Private Members

        private RoleRepository _roleRepository = new RoleRepository();
        private AppUserRepository _userRepository = new AppUserRepository();

        #endregion

        #region Properties

        private string _appName;
        public override string ApplicationName
        {
            get { return _appName; }
            set
            {
                _appName = value;

                if (_appName.Length > StringResources.Name_Max_Size)
                {
                    throw new ProviderException(StringResources.Provider_application_name_too_long);
                }
            }
        }

        private int _commandTimeout;
        private int CommandTimeout
        {
            get { return _commandTimeout; }
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Initializes the Entity Framework role provider with the property values specified in the application's configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            //TraceUtility.LogMethodEntry();

            if (String.IsNullOrEmpty(name))
                name = "L2ERoleProvider";

            base.Initialize(name, config);

            SecurityUtility.InitializeCommonParameters(config, out _appName, out _commandTimeout);
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, StringResources.Name_Max_Size, "roleName");
            SecurityUtility.CheckParameter(ref username, true, false, true, StringResources.Name_Max_Size, "username");

            if (username.Length < 1 || roleName.Length < 1)
                return false;

            return _roleRepository.IsUserInRole(username, roleName);
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {

            SecurityUtility.CheckParameter(ref username, true, false, true, StringResources.Name_Max_Size, "username");
            if (username.Length < 1)
                return new string[0];

            AppUser user = _userRepository.GetByUserName(username);

            if(!user.Role.IsLoaded)
                user.Role.Load();

            StringCollection roleList = new StringCollection();
            foreach (Role role in user.Role)
            {
                roleList.Add(role.RoleName);
            }

            String[] rolesForUser = new String[roleList.Count];
            roleList.CopyTo(rolesForUser, 0);
            return rolesForUser;
        }

        /// <summary>
        /// Gets a list of users in the specified role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override string[] GetUsersInRole(string roleName)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");

            Role role = _roleRepository.GetByRoleName(roleName);

            if (!role.AppUser.IsLoaded)
                role.AppUser.Load();

            StringCollection userList = new StringCollection();
            foreach (AppUser user in role.AppUser)
            {
                userList.Add(user.UserName);
            }

            String[] usersForRole = new String[userList.Count];
            userList.CopyTo(usersForRole, 0);
            return usersForRole;
        }

        /// <summary>
        /// Adds a new role to the data source
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, StringResources.Name_Max_Size, "roleName");

            if (RoleExists(roleName))
                throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Provider_role_already_exists, roleName));

            Role roleDetail = _roleRepository.New();
            roleDetail.RoleName = roleName;

            _roleRepository.Save(roleDetail);
        }

        /// <summary>
        /// Removes a role from the data source 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, StringResources.Name_Max_Size, "roleName");

            Role roleToDelete = _roleRepository.GetByRoleName(roleName);

            if (!roleToDelete.AppUser.IsLoaded)
                roleToDelete.AppUser.Load();

            if (throwOnPopulatedRole && 0 < roleToDelete.AppUser.Count)
            {
                //We do not want to delete all these users that are part of this role
                throw new ProviderException(StringResources.Role_is_not_empty);
            }

            _roleRepository.Delete(roleToDelete);
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, StringResources.Name_Max_Size, "roleName");

            List<Role> results = _roleRepository.FindByRoleName(roleName);
            return (0 < results.Count);
        }

        /// <summary>
        /// Adds the specified user names to the specified roles
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            SecurityUtility.CheckArrayParameter(ref roleNames, true, true, true, StringResources.Name_Max_Size, "roleNames");
            SecurityUtility.CheckArrayParameter(ref usernames, true, true, true, StringResources.Name_Max_Size, "usernames");

            Collection<AppUser> users = new Collection<AppUser>();
            Collection<Role> roles = new Collection<Role>();

            foreach (string username in usernames)
            {
                users.Add(_userRepository.GetByUserName(username));
                foreach (string rolename in roleNames)
                {
                    roles.Add(_roleRepository.GetByRoleName(rolename));
                }
            }
            AddUsersToRoles(users, roles);
        }

        /// <summary>
        /// Removes the specified user names from the specified roles
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            SecurityUtility.CheckArrayParameter(ref roleNames, true, true, true, StringResources.Name_Max_Size, "roleNames");
            SecurityUtility.CheckArrayParameter(ref usernames, true, true, true, StringResources.Name_Max_Size, "usernames");

            Collection<AppUser> users = new Collection<AppUser>();
            Collection<Role> roles = new Collection<Role>();

            foreach (string username in usernames)
            {
                users.Add(_userRepository.GetByUserName(username));
                foreach (string rolename in roleNames)
                {
                    roles.Add(_roleRepository.GetByRoleName(rolename));
                }
            }
            RemoveUsersFromRoles(users, roles);
        }

        /// <summary>
        /// Gets a list of all the roles 
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            StringCollection roleList = new StringCollection();

            List<Role> rolesFromDB = _roleRepository.GetAll();

            foreach (var roleDetail in rolesFromDB)
            {
                roleList.Add(roleDetail.RoleName);
            }

            string[] roles = new string[roleList.Count];
            roleList.CopyTo(roles, 0);
            return roles;
        }

        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match. 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            SecurityUtility.CheckParameter(ref roleName, true, true, true, StringResources.Name_Max_Size, "roleName");
            SecurityUtility.CheckParameter(ref usernameToMatch, true, true, false, StringResources.Name_Max_Size, "usernameToMatch");

            //TODO: Implement the Contains cop-out isn't very robust (should allow wildcards)
            //Also, the user list that is returned is supposed to be sorted alphabetically
            Role role = _roleRepository.GetByRoleName(roleName);

            if (!role.AppUser.IsLoaded)
                role.AppUser.Load();

            StringCollection userList = new StringCollection();
            foreach (AppUser user in role.AppUser)
            {
                if (user.UserName.ToUpperInvariant().Contains(usernameToMatch.ToUpperInvariant()))
                    userList.Add(user.UserName);
            }

            String[] usersForRole = new String[userList.Count];
            userList.CopyTo(usersForRole, 0);
            return usersForRole;
        }

        /// <summary>
        /// Adds the specified user names to the specified roles 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        public void AddUsersToRoles(Collection<AppUser> users, Collection<Role> roles)
        {
            foreach (AppUser u in users)
            {
                foreach (Role r in roles)
                {
                    _roleRepository.AddUserToRole(u, r);
                }
            }
            _roleRepository.Save();
        }

        /// <summary>
        /// Removes the specified user names from the specified roles 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        public void RemoveUsersFromRoles(Collection<AppUser> users, Collection<Role> roles)
        {
            foreach (AppUser u in users)
            {
                foreach (Role r in roles)
                {
                    _roleRepository.RemoveUserFromRole(u, r);
                }
            }
            _roleRepository.Save();
        }
        #endregion
    }


}



