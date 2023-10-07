using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeMVC.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        [Remote("IsMobileUnique", "Employee", AdditionalFields = "EmpId",
            ErrorMessage = "Mobile number already exists.")]
        public string Mobile { get; set; }
        [Required]
        [Remote("IsEmailUnique", "Employee", AdditionalFields = "EmpId",
            ErrorMessage = "Email already exists")]
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