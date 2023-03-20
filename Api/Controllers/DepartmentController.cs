using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DemoDynamicRolePolicy.IdentityAuth;
using Microsoft.AspNetCore.Hosting;
using DemoDynamicRolePolicy.AuthPolicy;

namespace DemoDynamicRolePolicy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _department;
        private readonly UserManager<ApplicationUser> _userManager;
        public DepartmentController(IDepartmentRepository department, UserManager<ApplicationUser> userManager)
        {
            _department = department;
            _userManager = userManager;
        }

        [Authorize(Policy = "AdminViewData")]
        [HttpGet]
        [Route("GetDepartment")]
        public IActionResult GetDepartment()
        {
            return Ok(_department.GetDepartment());
        }

        [HttpGet]
        [Route("GetDepartmentByID/{Id}")]
        public APIResponse GetDeptById(int Id)
        {
            var response = new APIResponse();
            try
            {
                var result = _department.GetDepartmentByID(Id);
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

        [Authorize(Policy = "AdminCreate")]
        [HttpPost]
        [Route("AddDepartment")]
        public APIResponse AddDepartment([FromBody] Department department)
        {
            var res = new APIResponse();
            try
            {
                var input = _department.AddDept(department);
                res.Success = true;
                res.Message = "Department added successfully";
                res.Data = input;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error:" + ex.Message;
                res.Data = null;
            }
            return res;
        }

        [Authorize(Policy = "AdminEdit")]
        [HttpPost]
        [Route("UpdateDept")]
        public async Task<IActionResult> UpdateContect([FromBody] Department user)
        {
            try
            {
                await _department.UpdateDept(user);
                return Json("1");
            }
            catch (Exception)
            {
                return Json("0");
            }
        }

        [Authorize(Policy = "AdminDelete")]
        [HttpDelete]
        [Route("DeleteDepartment/{Id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            int result = 0;
            try
            {
                result = await _department.DeleteDepartment(id);
                if (id == 0)
                {
                    return NotFound();
                }
                return Json("1");
            }
            catch (Exception)
            {
                return Json("0");
            }
        }
    }
}
