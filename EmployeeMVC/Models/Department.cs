using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeMVC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Designation> Designation { get; set; }
    }
}