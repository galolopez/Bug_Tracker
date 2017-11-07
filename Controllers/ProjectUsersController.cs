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
    public class ProjectUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper helper = new ProjectHelper();

        // GET: Developer Assignations
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult DevAssignations(int id)
        {
            // get a list of the projects in the DB
            var project = db.Projects.Find(id);
            // instantiate the view model
            var model = new ProjectUsersViewModel();
            model.AssignProjectUsers = new AssignProjectUsersViewModel();
            model.UnassignProjectUsers = new UnassignProjectUsersViewModel();
            // 
            model.AssignProjectUsers.ProjectId = project.Id;
            model.AssignProjectUsers.ProjectName = project.Name;
            var devsNotOnProject = helper.ListDevelopersNotOnProject(project.Id);
            model.AssignProjectUsers.Users = new MultiSelectList(devsNotOnProject.OrderBy(d => d.DisplayName), "Id", "DisplayName", null);
            //
            model.UnassignProjectUsers.ProjectId = project.Id;
            model.UnassignProjectUsers.ProjectName = project.Name;
            var devsOnProject = helper.ListDevelopersOnProject(project.Id);
            model.UnassignProjectUsers.Users = new MultiSelectList(devsOnProject.OrderBy(d => d.DisplayName), "Id", "DisplayName", null);

            return View(model);
        }

        // GET: ProjectUsers
        [Authorize(Roles = "Admin, Project Manager")]
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

        // POST: ProjectUsers/View Users On Project
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult ViewUserOnProject(UnassignProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.RemoveUserFromProject(userId, model.ProjectId);
                    }
                    return RedirectToAction("Index", "Projects");
                }
            }
            return View(model);
        }

        // GET: ProjectUsers/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AssignUsers(int id)
        {
            var project = db.Projects.Find(id);
            var model = new AssignProjectUsersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListDevelopersNotOnProject(id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: ProjectUsers/Assign Users
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsers(AssignProjectUsersViewModel model)
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
                return RedirectToAction("DevAssignations", "ProjectUsers", new { id = model.ProjectId });
            }
            return View(model);
        }

        // GET: Users/Unassign Roles
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult UnassignUsers(int id)
        {
            var project = db.Projects.Find(id);
            var model = new UnassignProjectUsersViewModel { ProjectId = id, ProjectName = project.Name };
            var userProjectList = helper.ListDevelopersOnProject(id);
            model.Users = new MultiSelectList(userProjectList.OrderBy(m => m.FirstName), "Id", "FirstName", null);

            return View(model);
        }

        // POST: Users/Assign Roles
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignUsers(UnassignProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string userId in model.SelectedUsers)
                    {
                        helper.RemoveUserFromProject(userId, model.ProjectId);
                    }
                }
                return RedirectToAction("DevAssignations", "ProjectUsers", new { id = model.ProjectId });
            }
            return View(model);
        }
    }
}