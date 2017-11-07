using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;
using Bug_Tracker.Helpers;

namespace Bug_Tracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper helper = new ProjectHelper();

        // GET: Projects
        [Authorize(Roles = "Admin, Project Manager, Developer, AdminTest")]
        public ActionResult Index()
        {
            List<Project> projects;

            if (!User.IsInRole("Admin"))
            {
                projects = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name).Projects.ToList();
            }
            else
            {
                projects = db.Projects.ToList();
            }

            var model = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                var temp = new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name
                };

                var pm = helper.GetProjectManager(project.Id);
                if (pm == null)
                {
                    temp.PMName = "Unassigned";
                }
                else
                {
                    temp.PMName = pm.DisplayName;
                }
                model.Add(temp);
            }            
            return View(model);
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, Project Manager, Developer, AdminTest")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager, AdminTest")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager, AdminTest")]
        public ActionResult Create([Bind(Include = "Id,Name")] Project projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager, AdminTest")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager, AdminTest")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, AdminTest")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, AdminTest")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
