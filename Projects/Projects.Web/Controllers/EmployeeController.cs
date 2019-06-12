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
    public class EmployeeController : Controller
    {
        private const int _itemsPerPage = 10;
        private IEmployeeService _employeeService;
        private IExecutorCompanyService _executorCompanyService;
        private IPositionService _positionService;

        public EmployeeController(IEmployeeService employeeService, IExecutorCompanyService executorCompanyService, IPositionService positionService)
        {
            _employeeService = employeeService;
            _executorCompanyService = executorCompanyService;
            _positionService = positionService;
        }

        public ActionResult AjaxEmployeeList(string sortOrder, int? page)
        {
            IEnumerable<EmployeeDTO> employeeDTOs = _employeeService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<EmployeeVM> employeeVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ExecutorCompanySortParam = sortOrder == "ExecutorCompany" ? "executorCompany_desc" : "ExecutorCompany";
            ViewBag.PositionSortParam = sortOrder == "Position" ? "position_desc" : "Position";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.FullName));
                    break;
                case "ExecutorCompany":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderBy(n => n.ExecutorCompany.Name));
                    break;
                case "executorCompany_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.ExecutorCompany.Name));
                    break;
                case "Position":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderBy(n => n.Position.Name));
                    break;
                case "position_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.Position.Name));
                    break;
                default:
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs);
                    break;
            }
            
            return PartialView(employeeVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }
        // GET: Employee
        public ActionResult Index(string sortOrder, int? page)
        {
            IEnumerable<EmployeeDTO> employeeDTOs = _employeeService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<EmployeeVM> employeeVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ExecutorCompanySortParam = sortOrder == "ExecutorCompany" ? "executorCompany_desc" : "ExecutorCompany";
            ViewBag.PositionSortParam = sortOrder == "Position" ? "position_desc" : "Position";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.FullName));
                    break;
                case "ExecutorCompany":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderBy(n => n.ExecutorCompany.Name));
                    break;
                case "executorCompany_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.ExecutorCompany.Name));
                    break;
                case "Position":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderBy(n => n.Position.Name));
                    break;
                case "position_desc":
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs.OrderByDescending(n => n.Position.Name));
                    break;
                default:
                    employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs);
                    break;
            }

            return View(employeeVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                EmployeeDTO employeeDTO = _employeeService.Get(id);
                EmployeeVM employeeVM = Mapper.Map<EmployeeVM>(employeeDTO);

                return View(employeeVM);
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
            ViewBag.ExecutorCompanyId = GetExecutorCompanyIdSelectList();
            ViewBag.PositionId = GetPositionIdSelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName, ExecutorCompanyId, PositionId, Phone, Email, DateBorn")] EmployeeVM employeeVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeDTO employeeDTO = Mapper.Map<EmployeeDTO>(employeeVM);
                    _employeeService.Add(employeeDTO);

                    return RedirectToAction("Index");
                }

                ViewBag.ExecutorCompanyId = GetExecutorCompanyIdSelectList(employeeVM.ExecutorCompanyId);
                ViewBag.PositionId = GetPositionIdSelectList(employeeVM.PositionId);

                return View(employeeVM);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                EmployeeDTO employeeDTO = _employeeService.Get(id);
                EmployeeVM employeeVM = Mapper.Map<EmployeeVM>(employeeDTO);

                ViewBag.ExecutorCompanyId = GetExecutorCompanyIdSelectList(employeeVM.ExecutorCompanyId);
                ViewBag.PositionId = GetPositionIdSelectList(employeeVM.PositionId);

                return View(employeeVM);
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
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id, FullName, ExecutorCompanyId, PositionId, Phone, Email, DateBorn")] EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                EmployeeDTO employeeDTO = Mapper.Map<EmployeeDTO>(employeeVM);
                _employeeService.Update(employeeDTO);

                return RedirectToAction("Index");
            }
            
            ViewBag.ExecutorCompanyId = GetExecutorCompanyIdSelectList(employeeVM.ExecutorCompanyId);
            ViewBag.PositionId = GetPositionIdSelectList(employeeVM.PositionId);

            return View(employeeVM);
        }

        public SelectList GetExecutorCompanyIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(_executorCompanyService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        public SelectList GetPositionIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(_positionService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _employeeService.Delete(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindEmployees(string value, string type)
        {
            value = value.Trim().ToLower();

            List<EmployeeDTO> employeeDTOs = _employeeService
                .GetEmployeesBy(type, value)
                .ToList();

            List<EmployeeVM> employeeVMs = Mapper.Map<IEnumerable<EmployeeVM>>(employeeDTOs).ToList();

            return PartialView(employeeVMs);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}