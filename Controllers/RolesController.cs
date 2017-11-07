using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();

        //GET: Role Assignations 
        [Authorize(Roles = "Admin, AdminTest")]
        public ActionResult RoleAssignations(string id)
        {
            // get a list of all roles in the DB
            var role = db.Roles.Find(id);
            // instantiate the view models
            var model = new RolesViewModel();
            model.AssignRoles = new AssignRolesViewModel();
            model.UnassignRoles = new UnassignRolesViewModel();

            model.AssignRoles.RoleId = role.Id;
            model.AssignRoles.RoleName = role.Name;
            var userRolesNotInList = helper.UsersNotInRole(role.Name);
            model.AssignRoles.Users = new MultiSelectList(userRolesNotInList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            model.UnassignRoles.RoleId = role.Id;
            model.UnassignRoles.RoleName = role.Name;
            var userRolesInList = helper.UsersInRole(role.Name);
            model.UnassignRoles.Users = new MultiSelectList(userRolesInList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);
            // send the model to the view
            return View(model);
        }

        // GET: Roles
        [Authorize(Roles = "Admin, AdminTest")]
        public ActionResult Index()
        {
            // get a list of all roles in the DB
            var roles = db.Roles.ToList();
            // instantiate the view models
            var model = new List<UserRolesViewModel>();
            // loop through all the roles in the DB and add a new RolesViewModel object for each one
            foreach(var role in roles)
            {
                model.Add(new UserRolesViewModel { RoleId = role.Id, RoleName = role.Name });
            }
            // send the model to the view
            return View(model);
        }

        // POST: ProjectUsers/AssignUsers
        [HttpPost]
        [Authorize(Roles = "Admin, AdminTest")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRoles(AssignRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.AddUserToRole(userId, model.RoleName);
                    }
                }
                return RedirectToAction("RoleAssignations", "Roles", new { id = model.RoleId });
            }
            return View(model);
        }

        // POST: Users/AssignRoles
        [HttpPost]
        [Authorize(Roles = "Admin, AdminTest")]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignRoles(UnassignRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.RemoveUserFromRole(id, model.RoleName);
                    }
                }
                return RedirectToAction("RoleAssignations", "Roles", new { id = model.RoleId });
            }
            return View(model);
        }
    }
}