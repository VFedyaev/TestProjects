using AutoMapper;
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

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult AjaxProjectList(string sortOrder, int? page)
        {
            IEnumerable<ProjectDTO> projectDTOs = _projectService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<ProjectVM> projectVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CompanySortParam = sortOrder == "Company" ? "company_desc" : "Company";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.PrioritySortParam = sortOrder == "Priority" ? "priority_desc" : "Priority";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Name));
                    break;
                case "Company":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.Customer.CompanyName));
                    break;
                case "company_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Customer.CompanyName));
                    break;
                case "Date":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.DateStart));
                    break;
                case "date_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.DateStart));
                    break;
                case "Priority":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.Priority));
                    break;
                case "priority_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Priority));
                    break;
                default:
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs);
                    break;
            }

            return PartialView(projectVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        // GET: Project
        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Index(string sortOrder, int? page)
        {
            IEnumerable<ProjectDTO> projectDTOs = _projectService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<ProjectVM> projectVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CompanySortParam = sortOrder == "Company" ? "company_desc" : "Company";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.PrioritySortParam = sortOrder == "Priority" ? "priority_desc" : "Priority";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Name));
                    break;
                case "Company":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.Customer.CompanyName));
                    break;
                case "company_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Customer.CompanyName));
                    break;
                case "Date":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.DateStart));
                    break;
                case "date_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.DateStart));
                    break;
                case "Priority":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderBy(n => n.Priority));
                    break;
                case "priority_desc":
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs.OrderByDescending(n => n.Priority));
                    break;
                default:
                    projectVMs = Mapper.Map<IEnumerable<ProjectVM>>(projectDTOs);
                    break;
            }

            return View(projectVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        [Authorize(Roles = "admin, manager, user")]
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

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Create()
        {
            ViewBag.CustomerId = GetCustomerIdSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager, user")]
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

        [Authorize(Roles = "admin, manager, user")]
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
        [Authorize(Roles = "admin, manager, user")]
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

        [Authorize(Roles = "admin, manager, user")]
        public SelectList GetCustomerIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(_customerService.GetAll().ToList(), "Id", "CompanyName", selectedValue);
        }

        [Authorize(Roles = "admin, manager, user")]
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
        [Authorize(Roles = "admin")]
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