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

        // GET: Roles
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            // get a list of all roles in the DB
            var roles = db.Roles.ToList();
            // instantiate the view model
            var model = new List<RolesViewModel>();
            // loop through all the roles in the DB and add a new RolesViewModel object for each one
            foreach(var role in roles)
            {
                model.Add(new RolesViewModel { RoleId = role.Id, RoleName = role.Name });
            }
            // send the model to the view
            return View(model);
        }

        // GET: ProjectUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult AssignRoles(string id)
        {
            var role = db.Roles.Find(id);
            var model = new RolesViewModel();
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            var userRolesList = helper.UsersNotInRole(role.Name);

            model.Users = new MultiSelectList(userRolesList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        // POST: ProjectUsers/AssignUsers
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRoles(RolesViewModel model)
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
                return RedirectToAction("Index", "Roles");
            }
            return View(model);
        }

        // GET: Users/UnassignedRoles
        [Authorize(Roles = "Admin")]
        public ActionResult UnassignRoles(string id)
        {
            var role = db.Roles.Find(id);
            var model = new RolesViewModel();
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            var userRolesList = helper.UsersInRole(role.Name);

            model.Users = new MultiSelectList(userRolesList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        // POST: Users/AssignRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignRoles(RolesViewModel model)
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
                return RedirectToAction("Index", "Roles");
            }
            return View(model);
        }
    }
}