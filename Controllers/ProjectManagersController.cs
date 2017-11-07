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

        // GET: PMAssignations
        [Authorize(Roles = "Admin")]
        public ActionResult PMAssignations(int id)
        {
            // get a list of the projects in the DB
            var project = db.Projects.Find(id);
            // instantiate the view model
            var model = new ProjectUsersViewModel();
            model.AssignProjectManagers = new AssignProjectManagersViewModel();
            model.UnassignProjectManagers = new UnassignProjectManagersViewModel();
            // 
            model.AssignProjectManagers.ProjectId = project.Id;
            model.AssignProjectManagers.ProjectName = project.Name;
            var PMNotOnProject = helper.ListManagersNotOnProject(project.Id);
            model.AssignProjectManagers.Users = new MultiSelectList(PMNotOnProject.OrderBy(d => d.DisplayName), "Id", "DisplayName", null);
            //
            model.UnassignProjectManagers.ProjectId = project.Id;
            model.UnassignProjectManagers.ProjectName = project.Name;
            var PMOnProject = helper.ListManagersOnProject(project.Id);
            model.UnassignProjectManagers.Users = new MultiSelectList(PMOnProject.OrderBy(d => d.DisplayName), "Id", "DisplayName", null);
            
            return View(model);
        }

        // GET: ProjectUsers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            // get a list of all roles in the DB
            var projects = db.Projects.ToList();
            // instantiate the view model
            var model = new List<ProjectViewModel>();
            // loop through all the roles in the DB and add a new RolesViewModel object for each one
            foreach (var project in projects)
            {
                model.Add(new ProjectViewModel { Id = project.Id, Name = project.Name });
            }
            // send the model to the view
            return View(model);
        }

        // GET: ProjectUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult AssignProjectManager(int id)
        {
            var project = db.Projects.Find(id);
            var model = new AssignProjectManagersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListManagersNotOnProject(id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: ProjectUsers/Assign Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProjectManager(AssignProjectManagersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    var user = helper.GetProjectManager(model.ProjectId);
                    if (user != null)
                    {
                        helper.RemoveUserFromProject(user.Id, model.ProjectId);
                    }
                    helper.AssignUserToProject(model.SelectedUsers[0], model.ProjectId);
                }
                return RedirectToAction("PMAssignations", "ProjectManagers", new { id = model.ProjectId });
            }
            return View(model);
        }

        // GET: ProjectUsers/Unassign Project Manager
        [Authorize(Roles = "Admin")]
        public ActionResult UnassignProjectManager(int id)
        {
            var project = db.Projects.Find(id);
            var model = new UnassignProjectManagersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListManagersOnProject(project.Id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: ProjectUsers/Unassign Project Manager
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignProjectManager(UnassignProjectManagersViewModel model)
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