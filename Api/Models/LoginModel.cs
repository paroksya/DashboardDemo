using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDynamicRolePolicy.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ?UserID { get; set; }
        public bool IsForAdmin { get; set; }
        public string ?roleName { get; set; }
    }
}
