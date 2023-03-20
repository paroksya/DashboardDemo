using DemoDynamicRolePolicy.IdentityAuth;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace DemoDynamicRolePolicy.Interface
{
    public interface IRoleRepository
    {
        Role AddRoles(Role input);

        List<Role> GetRoleNameList();

        List<Policy> GetPolicyNameList();

        IEnumerable<ApplicationUser> applicationUsers();

        Policy AddPolicies(Policy input);

        IEnumerable<Policy> GetPolicies();

        IEnumerable<RoleBasedPolicy> GetRolesBasedPolicies();

        void AddRoleBased(IList<RoleBasedPolicy> input);

        RoleBasedPolicy UpdateRoleBased(RoleBasedPolicy input);

        IEnumerable<RoleBasedPolicy> GetRolePolicyByID(int ID);

        List<RoleBasedPolicy> GetRolePolicyByIDs(int ID);

        RoleBasedPolicy GetRolePolicyByRoleID(int roleId);

        IEnumerable<RoleBasedPolicy> GetRolePolicyByIDs(int roleId, int policyId);

        List<RoleBasedPolicy> DeleteMultipleRoleBasedPolicies(IList<int> policies, int roleId);

        void AddRoleBasedPolicy(List<RoleBasedPolicy> input);

        void UpdateUserRole(IList<ApplicationUser> input);

        void DeleteMultipleUserRole(IList<ApplicationUser> input);

        IEnumerable<ApplicationUser> GetUserByID(string ID);
    }
}
