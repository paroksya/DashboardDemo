using DemoDynamicRolePolicy.IdentityAuth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDynamicRolePolicy.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public int DepartmentId { get; set; }

        public string EmailId { get; set; }

        [DisplayFormat(DataFormatString ="{yyyy-MM-dd}")]
        public DateTime DOJ { get; set; }

        public string ?PhotoFileName { get; set; }

        [NotMapped]
        public List<IFormFile> ?ThumbFile { get; set; }

        [NotMapped]
        public List<IFormFile> Files { get; set; }

        public Department ?Department { get; set; }

        [NotMapped]
        public string ?DepartmentName { get; set; }
    }
}
