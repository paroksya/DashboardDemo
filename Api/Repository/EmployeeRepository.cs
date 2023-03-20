using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using DemoDynamicRolePolicy.DBContext;

namespace DemoDynamicRolePolicy.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await (from e in _context.Employee
                          join d in _context.Department
                          on e.DepartmentId equals d.DepartmentId
                          into Emp
                          from d in Emp.DefaultIfEmpty()
                          select new Employee()
                          {
                              EmployeeID = e.EmployeeID,
                              EmployeeName = e.EmployeeName,
                              EmailId = e.EmailId,
                              DepartmentName = d.DepartmentName,
                              DOJ = e.DOJ,
                              PhotoFileName = e.PhotoFileName,
                          }).OrderByDescending(e => e.EmployeeID).ToListAsync();
        }

        public Employee GetEmpByID(int ID)
        {
            try
            {
                var result = (from e in _context.Employee.Where(x => x.EmployeeID == ID).DefaultIfEmpty()
                              join d in _context.Department
                              on e.DepartmentId equals d.DepartmentId
                              into Emp
                              from d in Emp
                              select new Employee()
                              {
                                  EmployeeID = e.EmployeeID,
                                  EmployeeName = e.EmployeeName,
                                  EmailId = e.EmailId,
                                  DepartmentId = d.DepartmentId,
                                  DOJ = e.DOJ,
                                  PhotoFileName = e.PhotoFileName,
                                  DepartmentName = d.DepartmentName
                              }).ToList();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Employee AddEmployees(Employee input)
        {
            _context.Employee.Add(input);
            _context.SaveChanges();
            return input;
        }

        public Employee UpdateEmployees(Employee input)
        {
            _context.Entry(input).State = EntityState.Modified;
            _context.SaveChanges();
            return input;
        }

        public async Task<int> DeleteEmployee(int ID)
        {
            int result = 0;
            if (_context != null)
            {
                var emp = await _context.Employee.FirstOrDefaultAsync(x => x.EmployeeID == ID);
                if (emp != null)
                {
                    _context.Employee.Remove(emp);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public List<Department> GetDeptNameList()
        {
            var dept = _context.Department.ToList();
            return dept;
        }

        public List<Employee> GetEmployeesExport(string search)
        {
            var emp = (from e in _context.Employee
                       join d in _context.Department on e.DepartmentId equals d.DepartmentId
                       into Emp
                       from d in Emp
                       where (e.EmployeeName.Contains(search) || e.EmailId.Contains(search) || d.DepartmentName.Contains(search))
                       select new Employee()
                       {
                           EmployeeID = e.EmployeeID,
                           EmployeeName = e.EmployeeName,
                           EmailId = e.EmailId,
                           DepartmentId = d.DepartmentId,
                           DOJ = e.DOJ,
                           DepartmentName = d.DepartmentName
                       }).ToList();
            return emp;
        }
    }
}
