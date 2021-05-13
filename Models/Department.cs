using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team2Geraldton.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Timings { get; set; }
        public string Description { get; set; }
    }

    public class DepartmentDto
    {
        public int DepartmentID { get; set; }

        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }
        [DisplayName("Timings")]
        public string Timings { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}