using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeMVC.Models
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }

        //Navigation Properties
        public virtual Department Department { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}