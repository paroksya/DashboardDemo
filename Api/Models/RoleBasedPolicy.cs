using DemoDynamicRolePolicy.IdentityAuth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoDynamicRolePolicy.Models
{
    public class RoleBasedPolicy
    {
        [Key]
        public int Id { get; set; }

        public int roleId { get; set; }

        public int policyID { get; set; }

        public bool isChecked { get; set; }

        [NotMapped]
        public string ?roleName { get; set; }

        [NotMapped]
        public string ?policyName { get; set; }
    }
}
