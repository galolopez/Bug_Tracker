namespace Bug_Tracker.Migrations
{
    using Bug_Tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bug_Tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "glopez19@live.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "glopez19@live.com",
                    Email = "glopez19@live.com",
                    FirstName = "Galo",
                    LastName = "Lopez",
                    DisplayName = "Galo"
                }, "Cuentame1!");
            }

            var userId = userManager.FindByEmail("glopez19@live.com").Id;
            userManager.AddToRole(userId, "Admin");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "bdavis@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "bdavis@coderfoundry.com",
                    Email = "bdavis@coderfoundry.com",
                    FirstName = "Bobby",
                    LastName = "Davis",
                    DisplayName = "Bobby Davis"
                }, "password1");
            }

            userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "jack@kim.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Jack",
                    Email = "jack@kim.com",
                    FirstName = "Jack",
                    LastName = "Kim",
                    DisplayName = "Jack Kim"
                }, "Cuentame1!");
            }

            userId = userManager.FindByEmail("jack@kim.com").Id;
            userManager.AddToRole(userId, "Project Manager");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "justin@lee.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Justin",
                    Email = "justin@lee.com",
                    FirstName = "Justin",
                    LastName = "Lee",
                    DisplayName = "Justin Lee"
                }, "Cuentame1!");
            }

            userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "araynor@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "araynor@coderfoundry.com",
                    Email = "araynor@coderfoundry.com",
                    FirstName = "Antonio",
                    LastName = "Raynor",
                    DisplayName = "Antonio Raynor"
                }, "Abc&123!");
            }

            userId = userManager.FindByEmail("araynor@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Developer");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "carlos@santana.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Carlos",
                    Email = "carlos@santana.com",
                    FirstName = "Carlos",
                    LastName = "Santana",
                    DisplayName = "Carlos Santana"
                }, "Cuentame1!");
            }

            userId = userManager.FindByEmail("carlos@santana.com").Id;
            userManager.AddToRole(userId, "Developer");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "jorge@ramos.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Jorge",
                    Email = "jorge@ramos.com",
                    FirstName = "Jorge",
                    LastName = "Ramos",
                    DisplayName = "Jorge Ramos"
                }, "Cuentame1!");
            }

            userId = userManager.FindByEmail("jorge@ramos.com").Id;
            userManager.AddToRole(userId, "Developer");
            /*-----------------------------------------------------------------------------------------*/
            if (!context.Users.Any(u => u.Email == "ajensen@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Jensen",
                    DisplayName = "Andrew Jensen"
                }, "Abc&123!");
            }

            userId = userManager.FindByEmail("ajensen@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Submitter");
            /*-----------------------------------------------------------------------------------------*/

            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Low" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Medium"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Medium" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "High" });
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Urgent"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" });
            }

            if (!context.TicketTypes.Any(p => p.Name == "Bug"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Bug" });
            }
            if (!context.TicketTypes.Any(p => p.Name == "Enhancement"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Enhancement" });
            }
            if (!context.TicketTypes.Any(p => p.Name == "Feature"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Feature" });
            }
            if (!context.TicketTypes.Any(p => p.Name == "Unknown"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Unknown" });
            }

            if (!context.TicketStatuses.Any(p => p.Name == "New"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "New" });
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Open"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Open" });
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Resolved"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Resolved" });
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Archived"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Archived" });
            }
        }
    }
}
