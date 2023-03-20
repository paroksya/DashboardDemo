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
    public class DepartmentRepository : IDepartmentRepository
    {
        private ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetDepartment()
        {
            return _context.Department.ToList().OrderByDescending(x => x.DepartmentId);
        }

        public Department GetDepartmentByID(int ID)
        {
            var result = (from d in _context.Department.Where(x => x.DepartmentId == ID).DefaultIfEmpty()
                          select new Department()
                          {
                              DepartmentId = d.DepartmentId,
                              DepartmentName = d.DepartmentName
                          }).ToList();
            return result.FirstOrDefault();
        }

        public Department AddDept(Department department)
        {
            _context.Department.Add(department);
            _context.SaveChanges();
            return department;
        }

        public async Task UpdateDept(Department department)
        {
            _context.Department.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteDepartment(int ID)
        {
            int result = 0;
            if (_context != null)
            {
                var dept = await _context.Department.FirstOrDefaultAsync(x => x.DepartmentId == ID);
                if (dept != null)
                {
                    _context.Department.Remove(dept);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
    }
}
