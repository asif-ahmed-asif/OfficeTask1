using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EmployeeMVC.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
        [Required]
        [DisplayName("Designation")]
        public int DesignationId { get; set; }

        //Navigation Properties
        public virtual Department Department { get; set; }
        public virtual Designation Designation { get; set; }
    }
}