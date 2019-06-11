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

        public ActionResult AjaxCustomerList(int? page)
        {
            IEnumerable<CustomerDTO> customerDTOs = _customerService
              .GetListOrderedByName()
              .ToList();
            IEnumerable<CustomerVM> customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs);

            return PartialView(customerVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Index(int? page)
        {
            IEnumerable<CustomerDTO> customerDTOs = _customerService
                .GetListOrderedByName()
                .ToList();
            IEnumerable<CustomerVM> customerVMs = Mapper.Map<IEnumerable<CustomerVM>>(customerDTOs);

            return View(customerVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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