using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Project
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.User = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}