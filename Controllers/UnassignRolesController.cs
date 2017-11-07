using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class UnassignRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();
        
        // GET: UnassignRoles
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/UnassignedRoles
        [Authorize(Roles = "Admin")]
        public ActionResult UnassignRoles(string id)
        {
            var role = db.Roles.Find(id);
            var model = new RolesViewModel();
            model.UnassignRoles.RoleId = role.Id;
            model.UnassignRoles.RoleName = role.Name;
            var userRolesList = helper.UsersInRole(role.Name);

            model.UnassignRoles.Users = new MultiSelectList(userRolesList.OrderBy(m => m.DisplayName), "Id", "DisplayName", null);

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
                if (model.UnassignRoles.SelectedUsers != null)
                {
                    foreach (string id in model.UnassignRoles.SelectedUsers)
                    {
                        helper.RemoveUserFromRole(id, model.UnassignRoles.RoleName);
                    }
                }
                return RedirectToAction("Index", "Roles");
            }
            return View(model);
        }
    }
}