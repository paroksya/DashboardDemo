using Microsoft.AspNetCore.Authorization;

namespace DemoDynamicRolePolicy.AuthPolicy
{
    public class ShouldBeAReaderRequirement : IAuthorizationRequirement
    {
        public string policyName { get; private set; }

        public ShouldBeAReaderRequirement(string policyName)
        {
            policyName = policyName;
        }
    }
}
