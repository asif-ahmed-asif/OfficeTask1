﻿using EmployeeMVC.Data;
using EmployeeMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace EmployeeMVC.Controllers
{
    public class EmployeeController : Controller
    {

        public EmployeeController(){}
        private DataContext _db = new DataContext();

        //private readonly DataContext _db;
        //public EmployeeController(DataContext db)
        //{
        //    _db = db;
        //}

        public async Task<ActionResult> Index()
        {
            var employees = await _db.Employee
                .Include(d => d.Department)
                .Include(c => c.Designation)
                .ToListAsync();
            return View(employees);
        }

        public async Task<ActionResult> Create()
        {
            await ViewBagForDepartment();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetDesignations(int id)
        {
            var designations = await _db.Designation
                .Where(d => d.DepartmentId == id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(designations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employee.Add(employee);
                await _db.SaveChangesAsync();
                TempData["success"] = "Employee Successfully Added!";
                return RedirectToAction("Index");
            }
            else
            {
                await ViewBagForDepartment();
            }


            return View(employee);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _db.Employee.FindAsync(id);

            await ViewBagForDepartment();
            await ViewBagForDesignation(employee.DepartmentId);

            return View(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Employee employee)
        {
            if (employee.EmpId == 0)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                var employeeDetails = await _db.Employee.FindAsync(employee.EmpId);
                employeeDetails.Name = employee.Name;
                employeeDetails.Mobile = employee.Mobile;
                employeeDetails.Email = employee.Email;
                employeeDetails.DepartmentId = employee.DepartmentId;
                employeeDetails.DesignationId = employee.DesignationId;

                await _db.SaveChangesAsync();
                TempData["success"] = "Employee Successfully Updated!";
                return RedirectToAction("Index");
            }
            else
            {
                await ViewBagForDepartment();
                await ViewBagForDesignation(employee.DepartmentId);
            }
            return View();
        }

        private async Task<ActionResult> ViewBagForDepartment()
        {
            ViewBag.Departments = await _db.Department.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();

            return null;
        }
        private async Task<ActionResult> ViewBagForDesignation(int id)
        {
            ViewBag.Designations = await _db.Designation
                .Where(d => d.DepartmentId == id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _db.Employee.FindAsync(id);
            if (employee != null)
            {
                _db.Employee.Remove(employee);
                await _db.SaveChangesAsync();
                TempData["success"] = "Employee Successfully Removed!";
                return Json(new { success = true });
            }
            return View();
        }

        public async Task<JsonResult> IsEmailUnique(string email, int empId)
        {
            return Json(!await _db.Employee.AnyAsync(u => u.Email == email && u.EmpId != empId), JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> IsMobileUnique(string mobile, int empId)
        {
            return Json(!await _db.Employee.AnyAsync(u => u.Mobile == mobile && u.EmpId != empId), JsonRequestBehavior.AllowGet);
        }
    }
}