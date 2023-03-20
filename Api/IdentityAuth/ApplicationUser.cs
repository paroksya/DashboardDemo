using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDynamicRolePolicy.IdentityAuth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string ?provider { get; set; }
        public string ?photoUrl { get; set; }
        [NotMapped]
        public string ?Password { get; set; }
        public string? roleName { get; set; }
    }
}
