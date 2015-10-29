using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var model = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserProjects = user.Projects.ToList(),
                TicketsAssigned = user.TicketsAssigned.ToList(),
                TicketsOwned = user.TicketsOwned.ToList()
            };
            
            return View(model);
        }
    }
}