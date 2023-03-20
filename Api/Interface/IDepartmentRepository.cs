using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDynamicRolePolicy.Models;

namespace DemoDynamicRolePolicy.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartment();

        Department GetDepartmentByID(int ID);

        Department AddDept(Department department);

        Task UpdateDept(Department department);

        Task<int> DeleteDepartment(int ID);
    }
}
