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

        //[Authorize]
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //[Authorize]
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        [Authorize]
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
                UserRoles = new List<string>(),
                TicketsAssigned = user.TicketsAssigned.ToList(),
                TicketsOwned = user.TicketsOwned.ToList(),
                TicketNotifications = db.TicketHistories.Where(h => h.NotificationSeen == false && h.UserId == user.Id).ToList()
            };

            return View(model);
        }

        [Authorize]
        public int? GetNotifications(string name)
        {
            if (name != "" && name != null)
            {
                var num = db.TicketHistories.Where(h => h.NotificationSeen == false && h.User.UserName == name).Count();
                return num;
            }
            return null;
        }
    }
}