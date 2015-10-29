using Bug_Tracker.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Helpers;

namespace Bug_Tracker.Controllers
{
    public class ProjectManagersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper helper = new ProjectHelper();

        // GET: ProjectUsers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            // get a list of all roles in the DB
            var projects = db.Projects.ToList();
            // instantiate the view model
            var model = new List<ProjectUsersViewModel>();
            // loop through all the roles in the DB and add a new RolesViewModel object for each one
            foreach (var project in projects)
            {
                model.Add(new ProjectUsersViewModel { ProjectId = project.Id, ProjectName = project.Name });
            }
            // send the model to the view
            return View(model);
        }

        // GET: ProjectUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult AssignProjectManager(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUsersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListManagersNotOnProject(id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: ProjectUsers/Assign Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProjectManager(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.AssignUserToProject(userId, model.ProjectId);
                    }
                }
                return RedirectToAction("Index", "ProjectManagers");
            }
            return View(model);
        }

        // GET: ProjectUsers/Unassign Project Manager
        [Authorize(Roles = "Admin")]
        public ActionResult UnassignProjectManager(int id)
        {
            var project = db.Projects.Find(id);
            var model = new ProjectUsersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListProjectManagers(project.Id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: ProjectUsers/Unassign Project Manager
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignProjectManager(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.RemoveUserFromProject(userId, model.ProjectId);
                    }
                    return RedirectToAction("Index", "ProjectManagers");
                }
            }
            return View(model);
        }

    }
}