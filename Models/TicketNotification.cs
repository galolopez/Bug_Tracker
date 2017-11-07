using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy, h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Changed { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}