using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoDynamicRolePolicy.AuthPolicy
{
    public class ShouldBeAnUserRequirementHandler : AuthorizationHandler<ShouldBeUserRequirement>
    {
        private readonly IRoleRepository _roleRepository;

        public ShouldBeAnUserRequirementHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldBeUserRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var empName = context.User.FindFirst(c => c.Type == ClaimTypes.Name);

            var claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claim.Value;

            var data = JsonConvert.DeserializeObject(role);

            if (data != null)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}