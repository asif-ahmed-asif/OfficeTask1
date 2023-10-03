﻿using EmployeeMVC.Data;
using EmployeeMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmployeeMVC.Controllers
{
    public class EmployeeController : Controller
    {
        public EmployeeController(){}
        private DataContext _db = new DataContext();

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
            ViewBag.Departments = await _db.Department.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();

            return View();
        }

        public async Task<ActionResult> GetDesgnations(int id)
        {
            var designations = await _db.Designation
                .Where(d => d.DepartmentId == id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(designations);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employee.Add(employee);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _db.Employee.FindAsync(id);

            ViewBag.Departments = await _db.Department.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();

            ViewBag.Designations = await _db.Designation
                .Where(d => d.DepartmentId == employee.DepartmentId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

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
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _db.Employee.FindAsync(id);
            if (employee != null)
            {
                _db.Employee.Remove(employee);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}