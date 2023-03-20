using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Authorization;
using DemoDynamicRolePolicy.AuthPolicy;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DemoDynamicRolePolicy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employee;
        private readonly IDepartmentRepository _department;
        private readonly IHostingEnvironment _env;
        public EmployeeController(IEmployeeRepository employee, IDepartmentRepository department, IHostingEnvironment env)
        {
            _employee = employee;
            _department = department;
            _env = env;
        }

        [Authorize(Policy = "UserViewData")]
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await _employee.GetEmployees());
        }


        [Authorize(Policy = "UserCreate")]
        [HttpPost]
        [Route("AddEmployees")]
        public APIResponse AddEmployees([FromForm] Employee input)
        {
            var response = new APIResponse();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var fullPath = string.Empty;
                    var webRootPath = _env.WebRootPath;
                    var newPath = Path.Combine(webRootPath, "Images");
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    for (var x = 0; x < Request.Form.Files.Count; x++)
                    {
                        var file = Request.Form.Files[x];
                        if (file.Length > 0)
                        {
                            string extention = Path.GetExtension(file.FileName);
                            if (FileType.ImageExtensions.Contains(extention.ToLower()))
                            {
                                fullPath = Path.Combine(newPath, file.FileName);
                            }
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                if (FileType.ImageExtensions.Contains(extention.ToLower()))
                                {
                                    input.PhotoFileName = file.FileName;
                                }
                            }
                        }
                    }
                    var emp = _employee.AddEmployees(input);
                    response.Success = true;
                    response.Message = "Employee added successfulyy";
                    response.Data = emp;
                }
                else
                {
                    response.Success = false;
                    response.Message = "File is not uploaded";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }

        [Authorize(Policy = "UserEdit")]
        [HttpPost]
        [Route("UpdateEmployees")]
        public APIResponse UpdateEmployee([FromForm] Employee input)
        {
            var response = new APIResponse();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    if (input.DepartmentId > 0)
                    {
                        var emp = _employee.GetEmpByID(input.EmployeeID);
                        if (emp != null && emp.EmployeeID > 0)
                        {
                            var fullPath = string.Empty;
                            var webRootPath = _env.WebRootPath;
                            var newPath = Path.Combine(webRootPath, "Images");
                            if (!Directory.Exists(newPath))
                            {
                                Directory.CreateDirectory(newPath);
                            }

                            for (var x = 0; x < Request.Form.Files.Count; x++)
                            {
                                var file = Request.Form.Files[x];
                                if (file.Length > 0)
                                {
                                    string extention = Path.GetExtension(file.FileName);
                                    if (FileType.ImageExtensions.Contains(extention.ToLower()))
                                    {
                                        fullPath = Path.Combine(newPath, file.FileName);
                                    }
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                        if (FileType.ImageExtensions.Contains(extention.ToLower()))
                                        {
                                            input.PhotoFileName = file.FileName;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var newEmp = _employee.UpdateEmployees(input);
                    response.Success = true;
                    response.Message = "Record Updated successfulyy";
                    response.Data = newEmp;
                }
                else
                {
                    response.Success = false;
                    response.Message = "File is not uploaded";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetEmpById/{Id}")]
        public APIResponse GetEmpById(int Id)
        {
            var response = new APIResponse();
            try
            {
                var result = _employee.GetEmpByID(Id);
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

        [Authorize(Policy = "UserDelete")]
        [HttpDelete]
        [Route("DeleteEmployee/{Id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            int result = 0;
            try
            {
                result = await _employee.DeleteEmployee(id);
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

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    stream.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [HttpGet]
        [Route("GetDeptNameList")]
        public APIResponse GetDeptNames()
        {
            var res = new APIResponse();
            try
            {
                var deptName = _employee.GetDeptNameList();
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


        [HttpGet]
        [Route("EmployeeListExport")]
        public APIResponse EmployeeExport([FromQuery] string search)
        {
            var res = new APIResponse();
            try
            {
                var empList = _employee.GetEmployeesExport((search == null) ? "" : search);
                res.Success = true;
                res.Message = null;
                res.Data = empList;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error:" + ex.Message;
                res.Data = null;
            }
            return res;
        }
    }
}
