using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDynamicRolePolicy.Models
{
    public class APIResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public int Count { get; set; }
        public int FilterRecCount { get; set; }
    }
}
