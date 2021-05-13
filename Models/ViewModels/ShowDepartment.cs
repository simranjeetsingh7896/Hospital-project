using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team2Geraldton.Models.ViewModels
{
    public class ShowDepartment
    {
        public bool isadmin { get; set; }
        public DepartmentDto department { get; set; }
    }
}