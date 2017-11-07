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
        public AssignRolesViewModel AssignRoles { get; set; }
        public UnassignRolesViewModel UnassignRoles { get; set; }
    }
}