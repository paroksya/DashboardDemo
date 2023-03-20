using System;
using System.ComponentModel.DataAnnotations;

namespace DemoDynamicRolePolicy.Models
{
    public class Policy
    {
        [Key]
        public int policyId { get; set; }
        public string policyName { get; set; }
    }
}
