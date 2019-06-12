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
    public class CustomerController : Controller
    {
        private const int _itemsPerPage = 10;
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult AjaxCustomerList(string sortOrder, int? page)
        {
            IEnumerable<CustomerDTO> customerDTOs = _customerService
              .GetListOrderedByName()
              .ToList();

            IEnumerable<CustomerVM> customerVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CompanySortParam = sortOrder == "Company" ? "company_desc" : "Company";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderByDescending(n => n.FullName));
                    break;
                case "Company":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderBy(n => n.CompanyName));
                    break;
                case "company_desc":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderByDescending(n => n.CompanyName));
                    break;
                default:
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs);
                    break;
            }
           

            return PartialView(customerVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Index(string sortOrder, int? page)
        {
            IEnumerable<CustomerDTO> customerDTOs = _customerService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<CustomerVM> customerVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CompanySortParam = sortOrder == "Company" ? "company_desc" : "Company";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderByDescending(n => n.FullName));
                    break;
                case "Company":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderBy(n => n.CompanyName));
                    break;
                case "company_desc":
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs.OrderByDescending(n => n.CompanyName));
                    break;
                default:
                    customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs);
                    break;
            }

            return View(customerVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                CustomerDTO customerDTO = _customerService.Get(id);
                CustomerVM customerVM = Mapper.Map<CustomerVM>(customerDTO);

                return View(customerVM);
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

        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Create([Bind(Include = "FullName, CompanyName, Phone, Email, Fax")] CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                CustomerDTO customerDTO = Mapper.Map<CustomerDTO>(customerVM);
                _customerService.Add(customerDTO);

                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                CustomerDTO customerDTO = _customerService.Get(id);
                CustomerVM customerVM = Mapper.Map<CustomerVM>(customerDTO);

                return View(customerVM);
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
        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit([Bind(Include = "Id, FullName, CompanyName, Phone, Email, Fax")] CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                CustomerDTO customerDTO = Mapper.Map<CustomerDTO>(customerVM);
                _customerService.Update(customerDTO);

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _customerService.Delete(id);
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