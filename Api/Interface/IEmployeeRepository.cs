using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDynamicRolePolicy.Models;

namespace DemoDynamicRolePolicy.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();

        Employee AddEmployees(Employee input);

        Employee UpdateEmployees(Employee input);

        Employee GetEmpByID(int ID);

        Task<int> DeleteEmployee(int ID);

        List<Department> GetDeptNameList();

        List<Employee> GetEmployeesExport(string search);
    }
}
