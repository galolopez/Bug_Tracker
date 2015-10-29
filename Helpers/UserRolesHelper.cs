using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Helpers
{
    public class UserRolesHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();    //limit number of connections to db, while staying local
        
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        public bool IsUserInRole (string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }

        public IList<string> ListUserRoles (string userId)
        {
            return manager.GetRoles(userId);
        }

        public bool AddUserToRole (string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole (string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            //return list of uers assigned to a single role
            var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;            
            return manager.Users.Where(u => u.Roles.Any(r => r.RoleId.Equals(roleId))).ToList();
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            //return list of uers assigned to a single role
            var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;
            return manager.Users.Where(u => !u.Roles.Any(r => r.RoleId.Equals(roleId))).ToList();
        }
    }
}