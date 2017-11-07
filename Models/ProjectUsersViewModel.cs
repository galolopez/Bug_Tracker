using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Models
{
    public class ProjectUsersViewModel
    {
        public AssignProjectUsersViewModel AssignProjectUsers { get; set; }
        public UnassignProjectUsersViewModel UnassignProjectUsers { get; set; }
        public AssignProjectManagersViewModel AssignProjectManagers { get; set; }
        public UnassignProjectManagersViewModel UnassignProjectManagers { get; set; }
    }

    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PMName { get; set; }
    }
}