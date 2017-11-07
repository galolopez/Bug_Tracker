using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;
using System.Threading.Tasks;

namespace Bug_Tracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);

            if (User.IsInRole("Admin"))
            {
                return View(tickets);
            }
            else if (User.IsInRole("Project Manager"))
            {
                return View(tickets.Where(t => t.Project.User.Any(u => u.UserName == User.Identity.Name)));
            }
            else if (User.IsInRole("Developer"))
            {
                return View(tickets.Where(t => t.AssignedUser.UserName == User.Identity.Name));
            }
            else
            {
                return View(tickets.Where(t => t.OwnerUser.UserName == User.Identity.Name));
            }
        }

        public ActionResult MarkNotificationSeen (int id)
        {
            var history = db.TicketHistories.Find(id);
            history.NotificationSeen = true;
            db.Entry(history).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            Ticket model = new Ticket(); 
            model.Created = new DateTimeOffset(DateTime.Now);
            model.OwnerUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            model.OwnerUserId = model.OwnerUser.Id;
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,Title,Description,OwnerUserId,AssignedUserId,Created,Updated")] Ticket tickets)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(tickets);
                tickets.Created = DateTimeOffset.Now;
                tickets.OwnerUser = db.Users.Single(u => u.UserName == User.Identity.Name);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }
       
        [Authorize(Roles = "Admin, Project Manager, Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            tickets.Updated = new DateTimeOffset(DateTime.Now);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);

            TempData["OldTicket"] = tickets;

            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager, Developer")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,Title,Description,OwnerUserId,AssignedUserId,Created,Updated")] Ticket tickets)
        {            
            if (ModelState.IsValid)
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
                var changedTime = new DateTimeOffset(DateTime.Now);
                Ticket oldTicket = (Ticket)TempData["OldTicket"];

                if (tickets.AssignedUserId != oldTicket.AssignedUserId)
                {
                    string temp = "";

                    if (oldTicket.AssignedUserId == null)
                    {
                        temp = "Unassigned";
                    }
                    else 
                    {
                        temp = oldTicket.AssignedUser.DisplayName;
                    }
                    
                    db.TicketHistories.Add(new TicketHistory
                        {
                            Changed = changedTime,
                            Property = "Assigned User",
                            TicketId = tickets.Id,
                            OldValue = temp,
                            NewValue = db.Users.Find(tickets.AssignedUserId).DisplayName,
                            UserId = user.Id
                        });
                }

                if (tickets.TicketTypeId != oldTicket.TicketTypeId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Type",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = db.TicketTypes.Find(tickets.TicketTypeId).Name,
                        UserId = user.Id
                    });
                }

                if (tickets.TicketPriorityId != oldTicket.TicketPriorityId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Priority",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = db.TicketPriorities.Find(tickets.TicketPriorityId).Name,
                        UserId = user.Id
                    });
                }

                if (tickets.TicketStatusId != oldTicket.TicketStatusId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Changed = changedTime,
                        Property = "Ticket Status",
                        TicketId = tickets.Id,
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = db.TicketStatuses.Find(tickets.TicketStatusId).Name,
                        UserId = user.Id
                    });
                }

                db.Entry(tickets).State = EntityState.Modified;
                tickets.Updated = DateTimeOffset.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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
