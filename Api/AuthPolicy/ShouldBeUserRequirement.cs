using Microsoft.AspNetCore.Authorization;

namespace DemoDynamicRolePolicy.AuthPolicy
{
    public class ShouldBeUserRequirement : IAuthorizationRequirement
    {
        public string policyName { get; private set; }

        public ShouldBeUserRequirement(string policyName)
        {
            policyName = policyName;
        }
    }
}
