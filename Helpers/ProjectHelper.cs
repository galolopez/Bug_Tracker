using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Helpers
{
    public class ProjectHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInProject (string userId, int projectId)
        {
            var proj = db.Projects.SingleOrDefault(p => p.Id == projectId);
            return proj.User.Any(u => u.Id.Equals(userId));            
        }

        public void AssignUserToProject(string userId, int projectId)
        {
            if(!IsUserInProject(userId, projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                proj.User.Add(user);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserInProject(userId, projectId))
            {
                var proj = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                proj.User.Remove(user);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            return db.Users.Find(userId).Projects.ToList();
        }

        public ICollection<ApplicationUser> ListDevelopersOnProject(int projectId)
        {
            // get Developer role ID
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Developer").Id;
            // get all users assigned to project
            var users = db.Projects.Find(projectId).User;
            // restrict user list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the list of devs
            return devs.ToList();
        }

        public ICollection<ApplicationUser> ListDevelopersNotOnProject(int projectId)
        {
            // get Developer role ID
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Developer").Id;
            // get all users NOT on the project
            var users = db.Users.Where(u => !u.Projects.Any(p => p.Id == projectId));
            // restrict user list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the list of devs
            return devs.ToList();
        }

        public ICollection<ApplicationUser> ListManagersOnProject(int projectId)
        {
            // get Developer role ID
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            // get all users assigned to project
            var users = db.Projects.Find(projectId).User;
            // restrict user list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the list of devs
            return devs.ToList();
        }

        public ICollection<ApplicationUser> ListManagersNotOnProject (int projectId)
        {
            // get Developer role ID
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            // get all users NOT on the project
            var users = db.Users.Where(u => !u.Projects.Any(p => p.Id == projectId));
            // restrict user list to developers only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the list of devs
            return devs.ToList();
        }

        public ApplicationUser GetProjectManager(int projectId)
        {
            // get PM role Id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            // get all users assigned to project
            var users = db.Projects.Find(projectId).User;
            // get the PM from the list of users
            var pm = users.SingleOrDefault(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the PM
            return pm;
        }

        public ICollection<ApplicationUser> ListAvailableProjectManagers (int projectId)
        {
            // get PM role id
            var roleId = db.Roles.SingleOrDefault(r => r.Name == "Project Manager").Id;
            // get all users NOT on the project
            var users = db.Users.Where(u => !u.Projects.Any(p => p.Id == projectId));
            // restrict user list to PMs only
            var devs = users.Where(user => user.Roles.Any(role => role.RoleId == roleId));
            // return the list of devs
            return devs.ToList();
        }

        public ICollection<Project> ListProjectsNotForUser(string userId)
        {
            return db.Projects.Where(p => !p.User.Any(u => u.Id == userId)).ToList();
        }
    }
}