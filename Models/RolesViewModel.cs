using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class UserRolesViewModel
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
    
    public class RolesViewModel
    {
        public string RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Users")]
        public System.Web.Mvc.MultiSelectList Users { get; set; }

        public string[] SelectedUsers { get; set; }
    }
}