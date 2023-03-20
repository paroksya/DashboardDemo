using Dapper;
using DemoDynamicRolePolicy.DBContext;
using DemoDynamicRolePolicy.IdentityAuth;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using ZendeskApi_v2.Models.Tickets;
using ZendeskApi_v2.Requests;

namespace DemoDynamicRolePolicy.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IGenericRepository<ApplicationUser> userRepository;
        private ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context, IGenericRepository<ApplicationUser> _userRepository)
        {
            _context = context;
            userRepository = _userRepository;
        }

        public Role AddRoles(Role input)
        {
            _context.Role.Add(input);
            _context.SaveChanges();
            return input;
        }

        public IEnumerable<ApplicationUser> applicationUsers()
        {
            var result = (from a in _context.applicationUsers
                          select new ApplicationUser()
                          {
                              Id = a.Id,
                              FirstName = a.FirstName,
                              LastName = a.LastName,
                              Email = a.Email,
                              UserName = a.UserName,
                              roleName = a.roleName,
                              IsActive = a.IsActive
                          }).ToList();
            return result;
        }

        public Policy AddPolicies(Policy input)
        {
            _context.Policy.Add(input);
            _context.SaveChanges();
            return input;
        }

        public IEnumerable<Policy> GetPolicies()
        {
            var result = (from a in _context.Policy
                          select new Policy()
                          {
                              policyId = a.policyId,
                              policyName = a.policyName,
                          }).ToList();
            return result;
        }

        public void AddRoleBased(IList<RoleBasedPolicy> input)
        {
            _context.RoleBasedPolicy.AddRange(input);
            _context.SaveChanges();
        }

        public RoleBasedPolicy UpdateRoleBased(RoleBasedPolicy input)
        {
            _context.Entry(input).State = EntityState.Modified;
            _context.SaveChanges();
            return input;
        }

        public List<Role> GetRoleNameList()
        {
            var role = _context.Role.ToList();
            return role;
        }

        public List<Policy> GetPolicyNameList()
        {
            var policy = _context.Policy.ToList();
            return policy;
        }

        public IEnumerable<RoleBasedPolicy> GetRolesBasedPolicies()
        {
            var result = (from a in _context.RoleBasedPolicy
                          join p in _context.Policy
                          on a.policyID equals p.policyId
                          join r in _context.Role
                          on a.roleId equals r.roleId
                          into RolePolicy
                          from r in RolePolicy.DefaultIfEmpty()
                          where a.roleId == r.roleId && a.isChecked == true
                          select new RoleBasedPolicy()
                          {
                              Id = a.Id,
                              roleId = a.roleId,
                              policyID = a.policyID,
                              policyName = p.policyName,
                              roleName = r.roleName,
                              isChecked = a.isChecked
                          }).ToList();
            return result;
        }

        public IEnumerable<RoleBasedPolicy> GetRolePolicyByID(int ID)
        {
            try
            {
                var result = (from a in _context.RoleBasedPolicy
                              join p in _context.Policy
                              on a.policyID equals p.policyId
                              join r in _context.Role
                              on a.roleId equals r.roleId
                              into RolePolicy
                              from r in RolePolicy.DefaultIfEmpty()
                              where a.roleId == ID
                              select new RoleBasedPolicy()
                              {
                                  Id = a.Id,
                                  roleId = a.roleId,
                                  policyID = a.policyID,
                                  policyName = p.policyName,
                                  roleName = r.roleName,
                                  isChecked = a.isChecked
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<RoleBasedPolicy> GetRolePolicyByIDs(int roleId, int policyId)
        {
            try
            {
                var result = (from a in _context.RoleBasedPolicy
                              join p in _context.Policy
                              on a.policyID equals p.policyId
                              join r in _context.Role
                              on a.roleId equals r.roleId
                              into RolePolicy
                              from r in RolePolicy.DefaultIfEmpty()
                              where a.policyID == policyId && a.roleId == roleId
                              select new RoleBasedPolicy()
                              {
                                  Id = a.Id,
                                  roleId = a.roleId,
                                  policyID = a.policyID,
                                  isChecked = a.isChecked
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public RoleBasedPolicy GetRolePolicyByRoleID(int roleId)
        {
            try
            {
                var result = (from a in _context.RoleBasedPolicy
                              join p in _context.Policy
                              on a.policyID equals p.policyId
                              join r in _context.Role
                              on a.roleId equals r.roleId
                              into RolePolicy
                              from r in RolePolicy.DefaultIfEmpty()
                              where a.roleId == roleId
                              select new RoleBasedPolicy()
                              {
                                  Id = a.Id,
                                  roleId = a.roleId,
                                  policyID = a.policyID,
                                  policyName = p.policyName,
                                  roleName = r.roleName,
                                  isChecked = a.isChecked
                              }).ToList();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<RoleBasedPolicy> GetRolePolicyByIDs(int ID)
        {
            try
            {
                var result = (from a in _context.RoleBasedPolicy
                              join p in _context.Policy
                              on a.policyID equals p.policyId
                              join r in _context.Role
                              on a.roleId equals r.roleId
                              into RolePolicy
                              from r in RolePolicy.DefaultIfEmpty()
                              where a.roleId == ID
                              select new RoleBasedPolicy()
                              {
                                  Id = a.Id,
                                  roleId = a.roleId,
                                  policyID = a.policyID,
                                  policyName = p.policyName,
                                  roleName = r.roleName,
                                  isChecked = a.isChecked
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<RoleBasedPolicy> DeleteMultipleRoleBasedPolicies(IList<int> policies, int roleId)
        {
            var result = (from a in _context.RoleBasedPolicy
                          join p in _context.Policy
                          on a.policyID equals p.policyId
                          join r in _context.Role
                          on a.roleId equals r.roleId
                          into RolePolicy
                          from r in RolePolicy.DefaultIfEmpty()
                          where a.roleId == roleId && policies.Contains(a.policyID)
                          select a).ToList();

            if (result != null)
            {
                _context.RoleBasedPolicy.RemoveRange(result);
                _context.SaveChanges();
            }
            return result;
        }

        public void AddRoleBasedPolicy(List<RoleBasedPolicy> input)
        {
            _context.RoleBasedPolicy.AddRange(input);
            _context.SaveChanges();
        }

        public void UpdateUserRole(IList<ApplicationUser> input)
        {
            var userDetail = userRepository.GetSingle(x => x.Id == input[0].Id);
            if (userDetail != null)
            {
                userDetail.roleName = input[0].roleName;
                _context.applicationUsers.UpdateRange(userDetail);
                _context.SaveChanges();
            }
        }

        public void DeleteMultipleUserRole(IList<ApplicationUser> input)
        {
            var userDetail = userRepository.GetSingle(x => x.Id == input[0].Id);

            if (userDetail != null)
            {

                userDetail.roleName = input[0].roleName;
                _context.applicationUsers.UpdateRange(userDetail);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ApplicationUser> GetUserByID(string ID)
        {
            try
            {
                var result = (from a in _context.applicationUsers.AsNoTracking()
                              where a.Id == ID
                              select new ApplicationUser()
                              {
                                  Id = a.Id,
                                  FirstName = a.FirstName,
                                  LastName = a.LastName,
                                  Email = a.Email,
                                  UserName = a.UserName,
                                  roleName = a.roleName,
                                  IsActive = a.IsActive
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
