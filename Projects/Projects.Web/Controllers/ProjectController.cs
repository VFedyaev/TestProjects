﻿using AutoMapper;
using PagedList;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class ProjectController : Controller
    {
        private const int _itemsPerPage = 10;
        private IProjectService _projectService;
        private ICustomerService _customerService;

        public ProjectController(IProjectService projectService, ICustomerService customerService)
        {
            _projectService = projectService;
            _customerService = customerService;
        }

        public ActionResult AjaxProjectList(int? page)
        {
            IEnumerable<ProjectDTO> projectDTOs = _projectService
                .GetListOrderedByName()
                .ToList();
            IEnumerable<ProjectVM> projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs);

            return PartialView(projectVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        // GET: Project
        public ActionResult Index(int? page)
        {
            IEnumerable<ProjectDTO> projectDTOs = _projectService
                .GetListOrderedByName()
                .ToList();
            IEnumerable<ProjectVM> projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs);

            return View(projectVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                ProjectDTO projectDTO = _projectService.Get(id);
                ProjectVM projectVM = Mapper.Map<ProjectVM>(projectDTO);

                return View(projectVM);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = GetCustomerIdSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, CustomerId, DateStart, DateEnd, Priority, Comment")] ProjectVM projectVM)
        {
            if (ModelState.IsValid)
            {
                ProjectDTO projectDTO = Mapper.Map<ProjectDTO>(projectVM);
                _projectService.Add(projectDTO);

                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = GetCustomerIdSelectList(projectVM.CustomerId);

            return View(projectVM);
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                ProjectDTO projectDTO = _projectService.Get(id);
                ProjectVM projectVM = Mapper.Map<ProjectVM>(projectDTO);

                ViewBag.CustomerId = GetCustomerIdSelectList(projectVM.CustomerId);

                return View(projectVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, CustomerId, DateStart, DateEnd, Priority, Comment")] ProjectVM projectVM)
        {
            if (ModelState.IsValid)
            {
                ProjectDTO projectDTO = Mapper.Map<ProjectDTO>(projectVM);
                _projectService.Update(projectDTO);

                return RedirectToAction("Index");
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения.");


            ViewBag.CustomerId = GetCustomerIdSelectList(projectVM.CustomerId);

            return View(projectVM);
        }

        public SelectList GetCustomerIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(_customerService.GetAll().ToList(), "Id", "CompanyName", selectedValue);
        }

        public ActionResult Employees(Guid? projectId)
        {
            try
            {
                IEnumerable<EmployeeDTO> employeeDTOs = _projectService.GetEmployees(projectId).ToList();
                IEnumerable<EmployeeVM> employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs);

                ViewBag.ProjectId = projectId;

                return View(employeeVMs);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _projectService.Delete(id);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (HasRelationsException)
            {
                return Content("Удаление невозможно.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}