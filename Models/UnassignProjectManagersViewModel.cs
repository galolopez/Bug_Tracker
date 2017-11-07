using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Models
{
    public class UnassignProjectManagersViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}