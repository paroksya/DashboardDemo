using DemoDynamicRolePolicy.DBContext;
using DemoDynamicRolePolicy.IdentityAuth;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using DemoDynamicRolePolicy.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZendeskApi_v2.Models.Sections;

namespace DemoDynamicRolePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePolicyController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private ApplicationDbContext _context;
        public RolePolicyController(IRoleRepository roleRepository, ApplicationDbContext context)
        {
            _roleRepository = roleRepository;
            _context = context;
        }

        [Route("AddRoles")]
        [HttpPost]
        public APIResponse AddRole([FromBody] Role model)
        {
            var res = new APIResponse();
            try
            {
                var input = _roleRepository.AddRoles(model);
                res.Success = true;
                res.Data = input;
                res.Message = "Roles Added Successfully";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Error:" + ex.Message;
            }
            return res;
        }

        [HttpGet]
        [Route("GetRoleNameList")]
        public APIResponse GetRolesNames()
        {
            var res = new APIResponse();
            try
            {
                var roleName = _roleRepository.GetRoleNameList();
                res.Success = true;
                res.Message = null;
                res.Data = roleName;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
                res.Data = null;
            }
            return res;
        }

        [Route("AddPolicy")]
        [HttpPost]
        public APIResponse Addpolicy([FromBody] Policy policy)
        {
            var res = new APIResponse();
            try
            {
                var permission = _roleRepository.AddPolicies(policy);
                res.Success = true;
                res.Data = policy;
                res.Message = "Policy Added Successfully";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Error:" + ex.Message;
            }
            return res;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUser()
        {
            return Ok(_roleRepository.applicationUsers());
        }

        [HttpPost]
        [Route("AddRoleBasedpolicy")]
        public APIResponse AddRoleBasedpolicy([FromBody] IList<RoleBasedPolicy> policy)
        {
            var res = new APIResponse();
            try
            {
                _roleRepository.AddRoleBased(policy);
                res.Success = true;
                res.Data = policy;
                res.Message = "Policy Added Successfully";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Error:" + ex.Message;
            }
            return res;
        }

        [HttpPost]
        [Route("DeleteMultipleRoleBasedPolicies")]
        public APIResponse DeleteMultipleRoleBasedPolicies([FromBody] IList<int> policies, int roleId)
        {
            var response = new APIResponse();
            try
            {
                var result = _roleRepository.DeleteMultipleRoleBasedPolicies(policies, roleId);
                response.Data = result;
                response.Message = "policy Updated Successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error : " + ex.Message;
                response.Data = null;
            }
            return response;
        }


        [HttpGet]
        [Route("GetPolicyNameList")]
        public APIResponse GetpolicyNames()
        {
            var res = new APIResponse();
            try
            {
                var deptName = _roleRepository.GetPolicyNameList();
                res.Success = true;
                res.Message = null;
                res.Data = deptName;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
                res.Data = null;
            }
            return res;
        }

        [Route("GetRolesBasedPolicies")]
        [HttpGet]
        public IActionResult GetRolesPolicies()
        {
            return Ok(_roleRepository.GetRolesBasedPolicies());
        }

        [HttpGet]
        [Route("GetRolePolicyByID/{Id}")]
        public APIResponse GetRoleBasedPolicyByID(int Id)
        {
            var response = new APIResponse();
            try
            {
                var result = _roleRepository.GetRolePolicyByID(Id);
                response.Success = true;
                response.Message = null;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public APIResponse UpdateUserRole([FromBody] IList<ApplicationUser> userRole)
        {
            var res = new APIResponse();
            try
            {
                _roleRepository.UpdateUserRole(userRole);
                res.Success = true;
                res.Data = userRole;
                res.Message = "UserRole Updated Successfully";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Data = null;
                res.Message = "Error:" + ex.Message;
            }
            return res;
        }

        [HttpGet]
        [Route("GetUserByID/{Id}")]
        public APIResponse GetUserByID(string Id)
        {
            var response = new APIResponse();
            try
            {
                var result = _roleRepository.GetUserByID(Id);
                response.Success = true;
                response.Message = null;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
    }
}
