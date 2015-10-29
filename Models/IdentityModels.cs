using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public ApplicationUser()
        {
            this.Projects               = new HashSet<Project>();
            this.TicketsOwned           = new HashSet<Ticket>();
            this.TicketsAssigned        = new HashSet<Ticket>();
        }

        public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty("OwnerUser")]
        public virtual ICollection<Ticket> TicketsOwned { get; set; }
        [InverseProperty("AssignedUser")]
        public virtual ICollection<Ticket> TicketsAssigned { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //inheritance : generic type class<to establish security>
    {
        public ApplicationDbContext()
            : base("AzureDataBase", throwIfV1Schema: false) //execute my parents constructors & passes connection string 
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}