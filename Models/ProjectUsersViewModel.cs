﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Models
{
    public class ProjectUsersViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        //[Display(Name = "Developers")]
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}